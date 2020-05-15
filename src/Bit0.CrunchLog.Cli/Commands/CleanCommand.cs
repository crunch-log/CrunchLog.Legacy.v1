using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.CleanCommand, Description = CliOptionKeys.CleanCommandDescription)]
    public class CleanCommand : CliBase
    {
        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<CleanCommand>((provider, logger, site) =>
            {
                logger.LogDebug(nameof(CleanCommand));

                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
            });
        }
    }
}