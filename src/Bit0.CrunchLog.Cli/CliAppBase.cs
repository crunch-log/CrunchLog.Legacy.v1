using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli
{
    [HelpOption(CliOptionKeys.HelpTemplate)]
    public abstract class CliAppBase : ICliApp
    {
        [Argument(0, CliOptionKeys.BasePathTemplate, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        public String BasePath { get; } = Environment.CurrentDirectory;

        [Option(CliOptionKeys.VerboseTemplate, Description = CliOptionKeys.VerboseDescription)]
        public LogLevel VerboseLevel { get; } = LogLevel.Information;

        public Boolean LoadConfig { get; }

        protected abstract Int32 OnExecute(CommandLineApplication app);

        public CliAppBase(Boolean loadConfig = false)
        {
#if DEBUG
            VerboseLevel = LogLevel.Trace;
            LoadConfig = loadConfig;
#endif
        }
    }
}