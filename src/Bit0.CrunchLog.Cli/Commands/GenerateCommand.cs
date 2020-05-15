using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.GenerateCommand, Description = CliOptionKeys.GenerateCommandDescription)]
    public class GenerateCommand : CliBase
    {
        [Argument(0, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = app.BuildArguments(BasePath, VerboseLevel);
            return app.Execute(args, (provider, logger) =>
            {
                logger.LogDebug(nameof(GenerateCommand));

                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.Publish();
            });
        }

    }
}