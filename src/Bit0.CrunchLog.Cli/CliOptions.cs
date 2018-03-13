using Bit0.CrunchLog.Cli.Extra;
using Bit0.CrunchLog.ContentTypes;
using Bit0.CrunchLog.Repositories;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.Cli
{
    [Command("crunch")]
    [Subcommand(Strings.GenerateCommand, typeof(GenerateCommand))]
    [Subcommand(Strings.CleanCommand, typeof(CleanCommand))]
    public class CliOptions : CliBase
    {
        [Option(Strings.VersionTemplate, Description = Strings.VersionDescription)]
        private Boolean ShowVersion { get; } = false;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            if (ShowVersion)
            {
                Console.WriteLine($"CrunchLog: {Version}");
                Console.WriteLine($"Runner: {RunnerVersion}");
                return 1;
            }

            WriteBanner();
            app.ShowHelp();
            return 1;
        }
    }

    [Command(Description = Strings.CleanCommandDescription)]
    public class CleanCommand : CliBase
    {
        [Option(Strings.VerboseTemplate, Description = Strings.VerboseDescription)]
        private LogLevel VerboseLevel { get; } = LogLevel.Information;

        [Argument(0, Description = Strings.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = BuildArguments(app, BasePath, VerboseLevel);
            return Execute(args, () =>
            {
                Logger.LogDebug("Clean");
                var generator = ServiceProvider.GetService<IContentGenerator>();
                generator.CleanOutput();

            });
        }
    }

    [Command(Description = Strings.GenerateCommandDescription)]
    public class GenerateCommand : CliBase
    {
        [Argument(0, Description = Strings.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        [Option(Strings.VerboseTemplate, Description = Strings.VerboseDescription)]
        private LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = BuildArguments(app, BasePath, VerboseLevel);
            return Execute(args, () =>
            {
                Logger.LogDebug("Generate");
                var generator = ServiceProvider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.PublishAll<Post>();
            });
        }

    }

    [HelpOption(Strings.HelpTemplate)]
    public abstract class CliBase
    {
        protected ILogger<CliOptions> Logger;
        protected IServiceProvider ServiceProvider;

        protected static String Version => GetVersion<CrunchLog>();
        protected static String RunnerVersion => GetVersion<CliOptions>();

        private static String GetVersion<TObject>() where TObject : class 
            => typeof(TObject).Assembly.GetName().Version.ToString();

        protected void WriteBanner()
        {
#if DEBUG
            var fc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("DEBUG BUILD ");
            Console.ForegroundColor = fc;
#endif
            Console.WriteLine(Strings.Banner, Version);
        }

        // ReSharper disable once UnusedMember.Global
        protected abstract Int32 OnExecute(CommandLineApplication app);

        protected Int32 Execute(Arguments args, Action executeFunc)
        {
            var sw = Stopwatch.StartNew();

            WriteBanner();

            try
            {
                ServiceProviderFactory.Build(args);

                ServiceProvider = ServiceProviderFactory.ServiceProvider;
                Logger = ServiceProvider.GetService<ILogger<CliOptions>>();

                executeFunc();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return 0;
            }

            sw.Stop();

            Logger.LogInformation($"Time elapsed: {sw.Elapsed}");

            return 1;
        }

        protected Arguments BuildArguments(CommandLineApplication app, String basePath = null, LogLevel verboseLevel = LogLevel.Information)
        {
            if (String.IsNullOrWhiteSpace(basePath))
            {
                basePath = app.WorkingDirectory;
            }

            return new Arguments
            {
                BasePath = basePath,
                VerboseLevel = verboseLevel
            };
        }
    }

}