using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Build(Arguments args)
        {
            
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder
                    .SetMinimumLevel(args.VerboseLevel)
                    .AddConsole();
            });
            services.AddSingleton(new JsonSerializer());
            services.AddSingleton(args);
            services.AddSingleton<CrunchLog>();
            services.AddSingleton(serviceProvider => serviceProvider.GetService<CrunchLog>().Config);
            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IContentGenerator, ContentGenerator>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
