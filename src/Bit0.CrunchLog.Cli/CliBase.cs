using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli
{
    [HelpOption(CliOptionKeys.HelpTemplate)]
    public abstract class CliBase : ICliApp
    {
        [Argument(0, CliOptionKeys.BasePathTemplate, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        public String BasePath { get; } = Environment.CurrentDirectory;

        [Option(CliOptionKeys.VerboseTemplate, Description = CliOptionKeys.VerboseDescription)]
        public LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected abstract Int32 OnExecute(CommandLineApplication app);

        public CliBase()
        {
#if DEBUG
            VerboseLevel = LogLevel.Trace;
#endif
        }
    }
}