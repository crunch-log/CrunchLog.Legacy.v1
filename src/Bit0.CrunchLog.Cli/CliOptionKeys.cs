using System;

namespace Bit0.CrunchLog.Cli
{
    public static class CliOptionKeys
    {
        public const String Banner = "CrunchLog, Static blog generator ({0})";

        #region Options
        public const String HelpTemplate = "-?|-h|--help";

        public const String VerboseTemplate = "-v|--verbose";
        public const String VerboseDescription = "Verbose level";
        
        public const String VersionTemplate = "--version";
        public const String VersionDescription = "Display CrunchLog version.";

        public const String BasePathTemplate = "--path";
        public const String BasePathDescription = "Directory with CrunchLog project";

        public const String UrlTemplate = "-u|--url|--set-url";
        public const String UrlDescription = "Url for the preview server";
        #endregion

        #region Commands
        public const String GenerateCommand = "generate";
        public const String GenerateCommandDescription = "Generate blog";

        public const String CleanCommand = "clean";
        public const String CleanCommandDescription = "Clean output";
        
        public const String RunCommand = "run";
        public const String RunCommandDescription = "Generate and Run in a preview server";

        public const String InitCommand = "init";
        public const String InitCommandDescription = "Creates an empty Crunchlog project";
        #endregion
    }
}
