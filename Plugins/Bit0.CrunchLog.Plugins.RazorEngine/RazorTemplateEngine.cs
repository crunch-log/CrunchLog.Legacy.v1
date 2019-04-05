using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ILogger<IRazorRenderer> _razorLogger;
        private readonly AppDomain _domain;

        public RazorTemplateEngine(CrunchSite siteConfig, ILogger<RazorTemplateEngine> logger, ILogger<IRazorRenderer> razorLogger)
        {
            _siteConfig = siteConfig;
            _logger = logger;
            _razorLogger = razorLogger;
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

        private IServiceProvider Build()
        {
            _logger.LogInformation("Create ServiceProvider");

            var services = new ServiceCollection();
            services.AddSingleton(_siteConfig);
            services.AddSingleton(_siteConfig.GetModel());
            services.AddSingleton(_razorLogger);
            services.AddScoped<IRazorRenderer, RazorRenderer>();

            return services.BuildServiceProvider();
        }

        public void Render(ITemplateModel model)
        {
            var outputDir = _siteConfig.Paths.OutputPath;

            //if (model is RedirectsTemplateModel)
            //{
            //    Render(model, "Redirect", outputDir.CombineFilePath(".json", "redirects"));
            //    return;
            //}
            if (model is PostTemplateModel m)
            {
                outputDir = !m.IsDraft ? outputDir.CombineDirPath(m.Id) : outputDir.CombineDirPath("draft", m.Id);
                Render(model, "Post", outputDir.CombineFilePath(".html", "index"));
                return;
            }
            if (model is PostListTemplateModel)
            {
                outputDir = outputDir.CombineDirPath(model.Permalink.Replace("//", "/").Substring(1));
                Render(model, "List", outputDir.CombineFilePath(".html", "index"));
                return;
            }

        }

        private void Render<TModel>(TModel model, String viewName, FileInfo outputFile) where TModel : ITemplateModel
        {
            if (!outputFile.Directory.Exists)
            {
                _logger.LogInformation($"Create directory: {outputFile.Directory.FullName}");
                outputFile.Directory.Create();
            }

            _logger.LogInformation($"Render template from view: {viewName}");

            using (var sw = outputFile.CreateText())
            {
                _renderer.RenderViewAsync($"{viewName}.cshtml", model, sw).GetAwaiter().GetResult();
            }
        }

        public void PreProcess() { }
        public void PostProcess(CrunchSite siteConfig, Theme theme) { }
    }
}
