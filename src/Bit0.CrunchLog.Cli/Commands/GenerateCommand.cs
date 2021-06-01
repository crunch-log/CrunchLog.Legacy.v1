using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;

namespace Bit0.CrunchLog.Cli.Commands
{
    [Command(CliOptionKeys.GenerateCommand, Description = CliOptionKeys.GenerateCommandDescription)]
    public class GenerateCommand : CliAppBase
    {
        [Option(CliOptionKeys.DotNotCleanContentTemplate, Description = CliOptionKeys.DotNotCleanContentDescription)]
        public Boolean DoNotCleanContent { get; } = false;

        [Option(CliOptionKeys.DotNotGenerateContentTemplate, Description = CliOptionKeys.DotNotGenerateContentDescription)]
        public Boolean DoNotGeneratenContent { get; } = false;

        public GenerateCommand() : base(loadConfig: true)
        { }

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            return this.Execute<GenerateCommand>(pipeline =>
            {
                pipeline.AddLog(LogLevel.Debug, nameof(GenerateCommand))
                    .AddCleanAction(cleanContent: !DoNotCleanContent || !DoNotGeneratenContent) // need to clean up output before generating new content
                    .AddGenerateAction(generateContent: !DoNotCleanContent || !DoNotGeneratenContent);
            });
        }

    }
}