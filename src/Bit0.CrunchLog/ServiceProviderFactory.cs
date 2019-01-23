using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Logging;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider Current { get; private set; }

        public static IServiceProvider Build(Arguments args)
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder
                    .SetMinimumLevel(args.VerboseLevel)
                    .AddConsole();
            });
            services.AddSingleton(args);

            services.AddSingleton<ConfigFile>();
            services.AddSingleton<CrunchLog>();
            services.AddSingleton(serviceProvider => serviceProvider.GetService<CrunchLog>().SiteConfig);
            //services.AddSingleton(serviceProvider => serviceProvider.GetService<CrunchLog>().Packages);

            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IContentGenerator, ContentGenerator>();
            services.AddTransient<ITemplateFactory, TemplateFactory>();

            // inject timestamps in log, in future replace with another logger
            services.Replace(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TimedLogger<>)));

            services.AddTransient<ITemplateEngine, JsonTemplateEngine>();
            // TODO: Add injection from plugins
            // move to plugin
            //services.Replace(ServiceDescriptor.Singleton(typeof(IHtmlTemplateEngine), typeof(HandelbarsTemplateEngine)));

            Current = services.BuildServiceProvider();

            return Current;
        }
    }
}
