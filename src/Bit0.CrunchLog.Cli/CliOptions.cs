using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace Bit0.CrunchLog.Cli
{
    [Command("crunch")]
    [Subcommand(CliOptionKeys.GenerateCommand, typeof(GenerateCommand))]
    [Subcommand(CliOptionKeys.CleanCommand, typeof(CleanCommand))]
    [Subcommand(CliOptionKeys.RunCommand, typeof(RunCommand))]
    public class CliOptions : CliBase
    {
        [Option(CliOptionKeys.VersionTemplate, Description = CliOptionKeys.VersionDescription)]
        private Boolean ShowVersion { get; } = false;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            if (ShowVersion)
            {
                Console.WriteLine($"CrunchLog: {app.GetVersion<CrunchLog>()}");
                Console.WriteLine($"Runner: {app.GetVersion<CliOptions>()}");
                return 1;
            }

            app.WriteBanner();
            app.ShowHelp();
            return 1;
        }
    }
}