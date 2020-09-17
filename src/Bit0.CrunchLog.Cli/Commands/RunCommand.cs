using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.RunCommand, Description = CliOptionKeys.RunCommandDescription)]
    public class RunCommand : CliAppBase
    {
        [Argument(1, CliOptionKeys.UrlTemplate, Description = CliOptionKeys.UrlDescription)]
        [DirectoryExists]
        private String Url { get; } = "http://localhost:3576/";

        public RunCommand() : base(loadConfig: true)
        { }

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<RunCommand>(pipeline =>
            {
                pipeline.AddLog(LogLevel.Debug, nameof(RunCommand))
                    .AddCleanAction()
                    .AddGenerateAction()
                    .AddRunAction(Url);
            });
        }
    }
}