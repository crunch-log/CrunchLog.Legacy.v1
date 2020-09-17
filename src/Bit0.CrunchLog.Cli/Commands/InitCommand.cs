using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
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
            return this.Execute<InitCommand>(pipeline =>
            {
                pipeline.AddLog(LogLevel.Debug, nameof(InitCommand))
                    .AddInitAction();
            });
        }
    }
}
