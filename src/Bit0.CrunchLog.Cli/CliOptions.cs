using Bit0.CrunchLog.Cli.Commands;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;

namespace Bit0.CrunchLog.Cli
{
    [Command("crunch", FullName = "CrunchLog, Static blog generator")]
    [VersionOptionFromMember(MemberName = nameof(Version))]
    [Subcommand(typeof(GenerateCommand))]
    [Subcommand(typeof(CleanCommand))]
    [Subcommand(typeof(RunCommand))]
    public class CliOptions : CliAppBase
    {
        //[Option(CliOptionKeys.VersionTemplate, Description = CliOptionKeys.VersionDescription)]
        //private Boolean ShowVersion { get; } = false;

        private String Version => FileVersionInfo.GetVersionInfo(typeof(CrunchLog).Assembly.Location).ProductVersion;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
        //    if (ShowVersion)
        //    {
        //        Console.WriteLine($"CrunchLog: {app.GetVersion<CrunchLog>()}");
        //        Console.WriteLine($"Runner: {app.GetVersion<Program>()}");
        //        return 1;
        //    }

            app.ShowHelp();
            return 1;
        }
    }
}