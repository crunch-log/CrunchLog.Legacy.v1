using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli
{
    [HelpOption(CliOptionKeys.HelpTemplate)]
    public abstract class CliBase
    {
        [Option(CliOptionKeys.VerboseTemplate, Description = CliOptionKeys.VerboseDescription)]
        protected LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected abstract Int32 OnExecute(CommandLineApplication app);
    }
}