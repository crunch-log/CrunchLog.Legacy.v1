using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.InitCommand, Description = CliOptionKeys.InitCommandDescription)]
    public class InitCommand : CliAppBase
    {
        [Option(CliOptionKeys.DevTemplate, Description = CliOptionKeys.DevDescription)]
        public Boolean IsDev { get; } = false;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<InitCommand>((provider, logger, crunch) =>
            {
            logger.LogDebug(nameof(InitCommand));

            var initializer = provider.GetService<IContentInitializer>();
            initializer.Generate();
            });
        }
    }
}
