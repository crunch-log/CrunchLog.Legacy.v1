using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.GenerateCommand, Description = CliOptionKeys.GenerateCommandDescription)]
    public class GenerateCommand : CliAppBase
    {
        public GenerateCommand() : base(loadConfig: true)
        { }

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<GenerateCommand>((provider, logger, crunch) =>
            {
                logger.LogDebug(nameof(GenerateCommand));

                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.Publish();
            });
        }

    }
}