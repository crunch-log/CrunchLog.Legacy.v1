using McMaster.Extensions.CommandLineUtils;
using System;

namespace Bit0.CrunchLog.Cli
{
    [HelpOption(CliOptionKeys.HelpTemplate)]
    public abstract class CliBase
    {
        protected abstract Int32 OnExecute(CommandLineApplication app);
    }
}