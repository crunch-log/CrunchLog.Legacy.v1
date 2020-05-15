using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.RunCommand, Description = CliOptionKeys.RunCommandDescription)]
    public class RunCommand : CliBase
    {
        [Argument(0, CliOptionKeys.BasePathTemplate, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        [Argument(1, CliOptionKeys.UrlTemplate, Description = CliOptionKeys.UrlDescription)]
        [DirectoryExists]
        private String Url { get; } = "";

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = app.BuildArguments(BasePath, VerboseLevel, Url);
            return app.Execute(args, (provider, logger, site) =>
            {
                logger.LogDebug(nameof(RunCommand));

                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.Publish();

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseWebRoot(site.Paths.OutputPath.FullName)
                    .Configure(config =>
                        config
                            .UseFileServer()
                            .UseDirectoryBrowser()
                            .UseStatusCodePagesWithRedirects("/{0}")
                        )
                        .UseUrls(Url)
                        .ConfigureLogging(logging =>
                            logging
                                .ClearProviders()
                                .AddConsole()
                        )
                        .Build();
                var task = host.RunAsync();

                app.OpenBrowser(Url);
                task.Wait();
            });
        }
    }
}