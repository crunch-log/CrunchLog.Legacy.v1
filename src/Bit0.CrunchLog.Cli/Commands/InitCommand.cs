using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.InitCommand, Description = CliOptionKeys.InitCommandDescription)]
    public class InitCommand : CliBase
    {

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<InitCommand>((provider, logger, site) =>
            {
                logger.LogDebug(nameof(InitCommand));

                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
            });
        }
    }
}
