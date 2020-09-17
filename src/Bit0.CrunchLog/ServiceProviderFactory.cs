using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Logging;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Factory;
using Bit0.CrunchLog.Template.ScribanEngine;
using Bit0.Plugins.Loader;
using Bit0.Plugins.Sdk;
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

            services.AddSingleton<JsonSerializer>();
            services.AddSingleton<CrunchLog>();

            services.AddSingleton(provider => provider.GetService<CrunchLog>().SiteConfig);
            services.AddSingleton<IPluginOptions>(provider => new PluginOptions
            {
                Directories = new[] { provider.GetService<CrunchConfig>().Paths.PluginsPath }
            });

            services.AddSingleton<IPackageManager, PackageManager>();
            services.AddSingleton<IContentProvider, ContentProvider>();
            services.AddSingleton<IContentGenerator, ContentGenerator>();
            services.AddSingleton<IContentInitializer, ContentInitializer>();
            services.AddSingleton<ITemplateFactory, TemplateFactory>();
            services.AddSingleton<ITemplateEngine, ScribanTemplateEngine>(); // Fix: JsonTemplate

            // load plugins
            services.LoadPlugins();

            return Current = services.BuildServiceProvider();
        }
    }
}
