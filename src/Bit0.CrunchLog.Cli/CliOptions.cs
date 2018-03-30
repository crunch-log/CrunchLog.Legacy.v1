using Bit0.CrunchLog.Cli.Extra;
using Bit0.CrunchLog.Repositories;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Bit0.CrunchLog.Config;
using Unosquare.Labs.EmbedIO;

namespace Bit0.CrunchLog.Cli
{
    [Command("crunch")]
    [Subcommand(Strings.GenerateCommand, typeof(GenerateCommand))]
    [Subcommand(Strings.CleanCommand, typeof(CleanCommand))]
    [Subcommand(Strings.RunCommand, typeof(RunCommand))]
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
            return Execute(args, (provider, logger) =>
            {
                logger.LogDebug("Clean");
                var generator = provider.GetService<IContentGenerator>();
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
            return Execute(args, (provider, logger) =>
            {
                logger.LogDebug("Generate");
                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.PublishAll();
            });
        }

    }

    [Command(Description = Strings.RunCommandDescription)]
    public class RunCommand : CliBase
    {
        [Argument(0, Description = Strings.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        [Argument(1, Description = Strings.UrlDescription)]
        [DirectoryExists]
        private String Url { get; } = "";

        [Option(Strings.VerboseTemplate, Description = Strings.VerboseDescription)]
        private LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = BuildArguments(app, BasePath, VerboseLevel, Url);
            return Execute(args, (provider, logger, config) =>
            {
                logger.LogDebug("Run");
                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.PublishAll();
                
                var server = WebServer
                    .Create(args.Url)
                    .EnableCors()
                    .WithLocalSession()
                    .WithStaticFolderAt(config.OutputPath.FullName);

                var cts = new CancellationTokenSource();
                var task = server.RunAsync(cts.Token);

                OpenBrowser(args.Url);

                Console.WriteLine("Press any key to close server.");
                Console.ReadKey(true);
                cts.Cancel();

                try
                {
                    task.Wait(cts.Token);
                } catch (AggregateException)
                {
                    // We'd also actually verify the exception cause was that the task
                    // was cancelled.
                    server.Dispose();
                }
            });
        }
    }

    [HelpOption(Strings.HelpTemplate)]
    public abstract class CliBase
    {
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

        protected Int32 Execute(
            Arguments args, 
            Action<IServiceProvider, ILogger<CliOptions>, CrunchConfig> executeFunc)
        {
            ILogger<CliOptions> logger = null;

            var sw = Stopwatch.StartNew();

            WriteBanner();

            try
            {
                ServiceProviderFactory.Build(args);

                var provider = ServiceProviderFactory.ServiceProvider;
                logger = provider.GetService<ILogger<CliOptions>>();
                var config = provider.GetService<CrunchConfig>();

                executeFunc(provider, logger, config);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return 0;
            }

            sw.Stop();

            logger.LogInformation($"Time elapsed: {sw.Elapsed}");

            return 1;
        }

        protected Int32 Execute(
            Arguments args,
            Action<IServiceProvider, ILogger<CliOptions>> executeFunc)
        {
            return Execute(args, (provider, logger, config) => executeFunc(provider, logger));
        }

        protected Arguments BuildArguments(
            CommandLineApplication app, 
            String basePath = null,
            LogLevel verboseLevel = LogLevel.Information, 
            String url = Arguments.UrlDefault)
        {
            if (String.IsNullOrWhiteSpace(basePath))
            {
                basePath = app.WorkingDirectory;
            }

            if (String.IsNullOrWhiteSpace(url))
            {
                url = Arguments.UrlDefault;
            }

            return new Arguments
            {
                BasePath = basePath,
                Url = url,
                VerboseLevel = verboseLevel
            };
        }

        protected static void OpenBrowser(String url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }

}