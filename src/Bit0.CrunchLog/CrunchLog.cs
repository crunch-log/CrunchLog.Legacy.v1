using Bit0.CrunchLog.Config;
using Bit0.Registry.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Bit0.CrunchLog
{
    public class CrunchLog
    {
        public CrunchSite SiteConfig { get; }

        public CrunchLog(Arguments arguments, ConfigFile configFile, ILogger<CrunchLog> logger, ILogger<CrunchSite> siteLogger, ILogger<PackageFeed> feedLogger)
        {
            var basePath = new DirectoryInfo(arguments.BasePath);
            logger.LogInformation($"Base path: {basePath}");

            SiteConfig = ReadConfigFile(configFile.File, logger, siteLogger, feedLogger);
        }

        private static CrunchSite ReadConfigFile(FileInfo configFile, ILogger<CrunchLog> logger, ILogger<CrunchSite> siteLogger, ILogger<PackageFeed> feedLogger)
        {
            logger.LogDebug($"Read configuration from: {configFile}");

            var siteConfig = new CrunchSite(siteLogger, feedLogger);
            JsonConvert.PopulateObject(configFile.OpenText().ReadToEnd(), siteConfig);

            logger.LogDebug("Configuration read");
            logger.LogInformation($"Output path: {siteConfig.Paths.OutputPath}");

            return siteConfig;
        }
    }
}
