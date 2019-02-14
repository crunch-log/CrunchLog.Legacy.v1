using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Logging;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Factory;
using Bit0.Plugins.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Bit0.CrunchLog
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider Current { get; private set; }

        public static IServiceProvider Build(Arguments args)
        {
            var jsonSerializer = new JsonSerializer();
            var configFile = ConfigFile.Load(args, jsonSerializer);

            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder
                    .SetMinimumLevel(args.VerboseLevel)
                    .AddConsole();
            });
            services.Replace(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TimedLogger<>)));  // TODO: replace with another logger

            services.AddSingleton(jsonSerializer);
            services.AddSingleton(configFile);
            services.AddSingleton<CrunchLog>();
            services.AddSingleton(factory => factory.GetService<CrunchLog>().SiteConfig);

            services.AddSingleton<IContentProvider, ContentProvider>();
            services.AddSingleton<IContentGenerator, ContentGenerator>();
            services.AddSingleton<ITemplateFactory, TemplateFactory>();
            services.AddSingleton<ITemplateEngine, JsonTemplateEngine>();

            services.LoadPlugins(new[] {
                configFile.Paths.PluginsPath,
                new FileInfo(Assembly.GetExecutingAssembly().Location).Directory
            }, new LoggerFactory().CreateLogger<IPluginLoader>());

            return Current = services.BuildServiceProvider();
        }
    }
}
