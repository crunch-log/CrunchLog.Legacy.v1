using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.CleanCommand, Description = CliOptionKeys.CleanCommandDescription)]
    public class CleanCommand : CliAppBase
    {
        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<CleanCommand>(pipeline =>
            {
                pipeline.AddLog(LogLevel.Debug, nameof(CleanCommand))
                    .AddCleanAction();
            });
        }
    }
}