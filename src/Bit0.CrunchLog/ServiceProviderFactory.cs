using Bit0.CrunchLog.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using Bit0.CrunchLog.Logging;
using Bit0.CrunchLog.ThemeHandler;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bit0.CrunchLog
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider Build(Arguments args)
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder
                    .SetMinimumLevel(args.VerboseLevel)
                    .AddConsole();
            });
            services.AddSingleton(new JsonSerializer()); // so that we can add global json config
            services.AddSingleton(args);

            services.AddSingleton<CrunchLog>();
            services.AddSingleton(serviceProvider => serviceProvider.GetService<CrunchLog>().Config);

            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IContentGenerator, ContentGenerator>();
            services.AddTransient<IThemeHandler, HandelbarsThemeHandler>();

            // inject timesamp in log, in future replace with another logger
            services.Replace(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TimedLogger<>)));

            return services.BuildServiceProvider();
        }
    }
}
