using System;
using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bit0.CrunchLog.Cli
{
    [Command(Description = CliOptionKeys.GenerateCommandDescription)]
    public class GenerateCommand : CliBase
    {
        [Argument(0, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        [Option(CliOptionKeys.VerboseTemplate, Description = CliOptionKeys.VerboseDescription)]
        private LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = app.BuildArguments(BasePath, VerboseLevel);
            return app.Execute(args, (provider, logger) =>
            {
                logger.LogDebug("Generate");
                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.Publish();
            });
        }

    }
}