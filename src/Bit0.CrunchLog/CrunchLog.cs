﻿using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Bit0.CrunchLog
{
    public class CrunchLog
    {
        public CrunchSite SiteConfig { get; }

        public CrunchLog(Arguments arguments, ConfigFile configFile, JsonSerializer jsonSerializer, ILogger<CrunchLog> logger)
        {
            var basePath = new DirectoryInfo(arguments.BasePath);
            logger.LogInformation($"Base path: {basePath}");

            SiteConfig = ReadConfigFile(configFile.File, jsonSerializer, logger);
        }

        private CrunchSite ReadConfigFile(FileInfo configFile, JsonSerializer jsonSerializer, ILogger<CrunchLog> logger)
        {
            logger.LogDebug($"Read configuration from: {configFile}");

            var siteConfig = new CrunchSite();
            jsonSerializer.Populate(configFile.OpenText(), siteConfig);

            logger.LogDebug("Configration read");
            logger.LogInformation($"Output path: {siteConfig.Paths.OutputPath}");

            return siteConfig;
        }
    }
}
