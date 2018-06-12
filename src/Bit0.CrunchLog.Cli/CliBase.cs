using System;
using McMaster.Extensions.CommandLineUtils;

namespace Bit0.CrunchLog.Cli
{
    [HelpOption(CliOptionKeys.HelpTemplate)]
    public abstract class CliBase
    {
        protected abstract Int32 OnExecute(CommandLineApplication app);
    }
}