using Bit0.CrunchLog.Config;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Bit0.CrunchLog.Cli.Extensions
{
    public static class CliAppExtensions
    {
        public static void WriteBanner(this CommandLineApplication app)
        {
#if DEBUG
            var fc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("DEBUG BUILD ");
            Console.ForegroundColor = fc;
#endif
            Console.WriteLine(CliOptionKeys.Banner, app.GetVersion<CrunchLog>());
        }

        public static Arguments BuildArguments(
            // ReSharper disable once UnusedParameter.Global
            this CommandLineApplication app,
            String basePath = null,
            // ReSharper disable once RedundantAssignment
            LogLevel verboseLevel = LogLevel.Information,
            String url = Arguments.UrlDefault)
        {
            if (String.IsNullOrWhiteSpace(basePath))
            {

#if DEBUG
                basePath = "Samples\\Site1";

                while (true)
                {
                    var dir = new DirectoryInfo(basePath);

                    if (dir.Exists)
                    {
                        basePath = dir.FullName;
                        break;
                    }

                    basePath = $"..\\{basePath}";
                }
#else

                basePath = app.WorkingDirectory;
#endif
            }

            if (String.IsNullOrWhiteSpace(url))
            {
                url = Arguments.UrlDefault;
            }

#if DEBUG
            verboseLevel = LogLevel.Trace;
#endif

            return new Arguments
            {
                BasePath = basePath,
                Url = url,
                VerboseLevel = verboseLevel
            };
        }

        // ReSharper disable once UnusedParameter.Global
        public static void OpenBrowser(this CommandLineApplication app, String url)
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

        public static Int32 Execute(
            this CommandLineApplication app,
            Arguments args,
            Action<IServiceProvider, ILogger<CliOptions>, CrunchConfig> executeFunc)
        {
            ILogger<CliOptions> logger = null;

            var sw = Stopwatch.StartNew();

            app.WriteBanner();

            try
            {
                var provider = ServiceProviderFactory.Build(args);

                logger = provider.GetService<ILogger<CliOptions>>();
                var config = provider.GetService<CrunchConfig>();

                executeFunc(provider, logger, config);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Server Closed");
                return 1;
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

        public static Int32 Execute(
            this CommandLineApplication app,
            Arguments args,
            Action<IServiceProvider, ILogger<CliOptions>> executeFunc)
        {
            return app.Execute(args, (provider, logger, config) => executeFunc(provider, logger));
        }

        // ReSharper disable once UnusedParameter.Global
        public static String GetVersion<TObject>(this CommandLineApplication app) where TObject : class
            => typeof(TObject).Assembly.GetName().Version.ToString();
    }
}
