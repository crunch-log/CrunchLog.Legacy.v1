using Bit0.CrunchLog.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Bit0.CrunchLog.Cli.Extensions
{
    public static class CliAppExtensions
    {
        public static void OpenBrowser(this Uri uri)
        {
            var url = uri.AbsoluteUri;

            try
            {
                Process.Start(url);
            }
            catch (Exception )
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

        public static Int32 Execute<T>(
            this ICliApp cli,
            Action<IServiceProvider, ILogger<T>, CrunchLog> executeFunc)
            where T : ICliApp
        {
            ILogger<T> logger = null;

            var sw = Stopwatch.StartNew();

            if (executeFunc == null)
            {
                throw new Exception("Cannot find Logger function.");
            }

            try
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                };

                var provider = ServiceProviderFactory.Build(new Arguments{
                    BasePath = cli.BasePath, 
                    LogLevel = cli.VerboseLevel,
                    LoadConfig = cli.LoadConfig
                });

                logger = provider.GetService<ILogger<T>>();

                var crunchLog = provider.GetService<CrunchLog>();

                executeFunc(provider, logger, crunchLog);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Server Closed");
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return 1;
            }

            sw.Stop();

            logger.LogInformation($"Time elapsed: {sw.Elapsed}");

            return 0;
        }
    }
}
