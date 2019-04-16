using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli
{
    [Command(CliOptionKeys.RunCommand, Description = CliOptionKeys.RunCommandDescription)]
    public class RunCommand : CliBase
    {
        [Argument(0, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        [Argument(1, Description = CliOptionKeys.UrlDescription)]
        [DirectoryExists]
        private String Url { get; } = "";

        [Option(CliOptionKeys.VerboseTemplate, Description = CliOptionKeys.VerboseDescription)]
        private LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = app.BuildArguments(BasePath, VerboseLevel, Url);
            return app.Execute(args, (provider, logger, config) =>
            {
                logger.LogDebug("Run");
                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.Publish();

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseWebRoot(config.Paths.OutputPath.FullName)
                    .Configure(x => x
                                        .UseFileServer()
                                        .UseDirectoryBrowser()
                                        .UseStatusCodePages()
                    )
                    .UseUrls(args.Url)
                    .ConfigureLogging(logging => logging
                                                    .ClearProviders()
                                                    .AddConsole()
                    )
                    .Build();
                var task = host.RunAsync();

                app.OpenBrowser(args.Url);
                task.Wait();
            });
        }
    }
}