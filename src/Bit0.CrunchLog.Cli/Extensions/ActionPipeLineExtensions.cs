using Bit0.CrunchLog.Cli.Commands;
using Bit0.PipeLines;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Extensions
{
    public static class ActionPipeLineExtensions
    {
        public static IActionPipeLine<ActionPipeLineContext<T>> AddLog<T>(this IActionPipeLine<ActionPipeLineContext<T>> pipeLine, LogLevel logLevel, String message, params Object[] args)
        {
            pipeLine.AddProcess(ctx => ctx.Logger.Log(logLevel, message, args));
            return pipeLine;
        }

        public static IActionPipeLine<ActionPipeLineContext<T>> AddCleanAction<T>(this IActionPipeLine<ActionPipeLineContext<T>> pipeLine)
        {
            pipeLine.AddProcess(ctx =>
            {
                var generator = ctx.ServiceProvider.GetService<IContentGenerator>();
                generator.CleanOutput();
            });
            return pipeLine;
        }

        public static IActionPipeLine<ActionPipeLineContext<T>> AddGenerateAction<T>(this IActionPipeLine<ActionPipeLineContext<T>> pipeLine)
        {
            pipeLine.AddProcess(ctx =>
            {
                var generator = ctx.ServiceProvider.GetService<IContentGenerator>();
                generator.Publish();
            });
            return pipeLine;
        }

        public static IActionPipeLine<ActionPipeLineContext<T>> AddRunAction<T>(this IActionPipeLine<ActionPipeLineContext<T>> pipeLine, String url)
        {
            pipeLine.AddProcess(ctx =>
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseWebRoot(ctx.CrunchLog.SiteConfig.Paths.OutputPath.FullName)
                    .Configure(config =>
                        config
                            .UseFileServer()
                            .UseDirectoryBrowser()
                            .UseStatusCodePagesWithRedirects("/{0}")
                        )
                        .UseUrls(url)
                        .ConfigureLogging(logging =>
                            logging
                                .ClearProviders()
                                .AddConsole()
                        )
                        .Build();
                var task = host.RunAsync();

                new Uri(url).OpenBrowser();
                task.Wait();
            });
            return pipeLine;
        }

        public static IActionPipeLine<ActionPipeLineContext<T>> AddInitAction<T>(this IActionPipeLine<ActionPipeLineContext<T>> pipeLine)
        {
            pipeLine.AddProcess(ctx =>
            {
                var initializer = ctx.ServiceProvider.GetService<IContentInitializer>();
                initializer.Generate();
            });
            return pipeLine;
        }
    }
}
