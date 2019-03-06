using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bit0.CrunchLog.Plugins.RazorEngine
{
    public class RazorTemplateEngine : ITemplateEngine
    {
        public readonly IRazorRenderer _renderer;
        private readonly CrunchSite _siteConfig;
        private readonly ILogger<RazorTemplateEngine> _logger;
        private readonly AppDomain _domain;

        public RazorTemplateEngine(CrunchSite siteConfig, ILogger<RazorTemplateEngine> logger)
        {
            _siteConfig = siteConfig;
            _logger = logger;
            _domain = AppDomain.CurrentDomain;

            _domain.AssemblyLoad += (sender, args) =>
            {
                _logger.LogDebug(new EventId(6000), $"Loaded: {args.LoadedAssembly.FullName}");
            };

            _domain.AssemblyResolve += (sender, args) =>
            {
                var dir = new FileInfo(args.RequestingAssembly.Location).Directory;
                var file = dir.GetFiles(args.Name.Split(',').FirstOrDefault() + ".dll", SearchOption.TopDirectoryOnly).FirstOrDefault();
                return Assembly.LoadFile(file.FullName);
            };

            try
            {
                var provider = Build();

                _renderer = provider.GetService<IRazorRenderer>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        private static IServiceProvider Build()
        {
            var application = PlatformServices.Default.Application;
            var services = new ServiceCollection();

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"c:\temp-keys"))
                .ProtectKeysWithDpapi();

            services.AddSingleton(application);

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = application.ApplicationName
            });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"));

            services.AddScoped<IRazorRenderer, RazorRenderer>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            });

            services.AddLogging();
            services.AddMvc();

            return services.BuildServiceProvider();
        }

        public void Render(ITemplateModel model)
        {
            var outputDir = _siteConfig.Paths.OutputPath;
            if (_siteConfig.Theme.OutputType == ThemeOutputType.Json)
            {
                outputDir = _siteConfig.Theme.Output.Data;
            }

            if (model is SiteTemplateModel)
            {
                Render(model, "Site", outputDir.CombineFilePath(".json", "siteInfo"));
            }
            else if (model is RedirectsTemplateModel)
            {
                Render(model, "Redirect", outputDir.CombineFilePath(".json", "redirects"));
            }
            else if (model is PostTemplateModel m)
            {
                outputDir = !m.IsDraft ? outputDir.CombineDirPath(m.Id) : outputDir.CombineDirPath("draft", m.Id);
                Render(model, "Post", outputDir.CombineFilePath(".json", "index"));
            }
            if (model is PostListTemplateModel)
            {
                outputDir = outputDir.CombineDirPath(model.Permalink.Replace("//", "/").Substring(1));
                Render(model, "List", outputDir.CombineFilePath(".json", "index"));
            }

        }

        private void Render<T>(T model, String viewName, FileInfo outputFile) where T : class
        {
            if (!outputFile.Directory.Exists)
            {
                outputFile.Directory.Create();
            }

            using (var sw = outputFile.CreateText())
            {
                _renderer.RenderViewAsync($"{viewName}.cshtml", model, sw).GetAwaiter().GetResult();
            }
        }
    }
}
