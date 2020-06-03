using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Logging;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Factory;
using Bit0.Plugins.Loader;
using Bit0.Registry.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Bit0.CrunchLog
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider Current { get; private set; }

        public static IServiceProvider Build(Arguments args)
        {
            var jsonSerializer = new JsonSerializer();
            var services = new ServiceCollection();

            // setup
            var packsDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".packs"));
            var pakageManager = new PackageManager(packsDir, new WebClient(), new LoggerFactory().CreateLogger<IPackageManager>());
            var crunch = new CrunchLog(args, jsonSerializer, pakageManager, new LoggerFactory().CreateLogger<CrunchLog>());

            // add logger
            services.AddLogging(builder =>
            {
                builder
                    .SetMinimumLevel(args.LogLevel)
                    .AddConsole();
            });
            services.Replace(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TimedLogger<>)));  // TODO: replace with another logger

            // add to IoC
            services.AddSingleton(args);
            services.AddSingleton(jsonSerializer);
            services.AddSingleton(crunch);
            services.AddSingleton(crunch.SiteConfig);
            services.AddSingleton<IPackageManager>(pakageManager);

            services.AddSingleton<IContentProvider, ContentProvider>();
            services.AddSingleton<IContentGenerator, ContentGenerator>();
            services.AddSingleton<IContentInitializer, ContentInitializer>();
            services.AddSingleton<ITemplateFactory, TemplateFactory>();
            services.AddSingleton<ITemplateEngine, JsonTemplateEngine>(); // Fix: JsonTemplate

            // load plugins
            services.LoadPlugins(new[] {
                crunch.SiteConfig.Paths.PluginsPath,
            }, new LoggerFactory().CreateLogger<IPluginLoader>());

            return Current = services.BuildServiceProvider();
        }
    }
}
