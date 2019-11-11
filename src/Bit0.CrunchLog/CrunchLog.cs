using Bit0.CrunchLog.Config;
using Bit0.Registry.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog
{
    public class CrunchLog
    {
        public CrunchSite SiteConfig { get; }

        public CrunchLog(IConfigFile configFile, Arguments arguments, JsonSerializer jsonSerializer, IPackageManager packageManager, ILogger<CrunchLog> logger, ILogger<CrunchSite> siteLogger)
        {
            logger.LogInformation($"Base path: {configFile.Paths.BasePath}");

            SiteConfig = ReadConfigFile(configFile, arguments.Url, jsonSerializer, packageManager, logger, siteLogger);
        }

        private static CrunchSite ReadConfigFile(IConfigFile configFile, String baseUrl, JsonSerializer jsonSerializer, IPackageManager packageManager, ILogger<CrunchLog> logger, ILogger<CrunchSite> siteLogger)
        {
            logger.LogDebug($"Read configuration from: {configFile}");

            var siteConfig = new CrunchSite(configFile.Paths, baseUrl, packageManager, siteLogger);
            jsonSerializer.Populate(configFile.JsonObject.CreateReader(), siteConfig);

            logger.LogDebug("Configuration read");
            logger.LogInformation($"Output path: {siteConfig.Paths.OutputPath}");

            return siteConfig;
        }
    }
}
