using System;

namespace Bit0.CrunchLog.Cli.Extra
{
    public static class Strings
    {
        public const String Banner = "CrunchLog, Static blog generator ({0})";

        public const String VersionTemplate = "--version";
        public const String HelpTemplate = "-?|-h|--help|--h";
        public const String VerboseTemplate = "-v|--verbose";
        
        public const String VersionDescription = "Display CrunchLog version.";

        public const String GenerateCommand = "generate";
        public const String GenerateCommandDescription = "Generate blog";
        public const String BasePathDescription = "Directory with CrunchLog project";

        public const String CleanCommand = "clean";
        public const String CleanCommandDescription = "Clean output";

        public const String VerboseDescription = "Verbose level";
        
        public const String RunCommand = "run";
        public const String RunCommandDescription = "Generate and Run in a preview server";
        public const String UrlDescription = "Url for the preview server";
    }
}
