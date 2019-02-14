using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Bit0.CrunchLog
{
    public class CrunchLog
    {
        public CrunchSite SiteConfig { get; }

        public CrunchLog(IConfigFile configFile, JsonSerializer jsonSerializer, ILogger<CrunchLog> logger, ILogger<CrunchSite> siteLogger)
        {
            logger.LogInformation($"Base path: {configFile.Paths.BasePath}");

            SiteConfig = ReadConfigFile(configFile, jsonSerializer, logger, siteLogger);
        }

        private static CrunchSite ReadConfigFile(IConfigFile configFile, JsonSerializer jsonSerializer, ILogger<CrunchLog> logger, ILogger<CrunchSite> siteLogger)
        {
            logger.LogDebug($"Read configuration from: {configFile}");

            var siteConfig = new CrunchSite(configFile.Paths, siteLogger);
            jsonSerializer.Populate(configFile.JsonObject.CreateReader(), siteConfig);

            logger.LogDebug("Configuration read");
            logger.LogInformation($"Output path: {siteConfig.Paths.OutputPath}");

            return siteConfig;
        }
    }
}
