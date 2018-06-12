using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Bit0.CrunchLog
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CrunchLog
    {
        public CrunchConfig Config { get; }

        public CrunchLog(Arguments arguments, ConfigFile configFile, JsonSerializer jsonSerializer, ILogger<CrunchLog> logger)
        {
            var basePath = new DirectoryInfo(arguments.BasePath);
            logger.LogInformation($"Base path: {basePath}");

            Config = ReadConfigFile(configFile.File, jsonSerializer, logger);
        }

        private CrunchConfig ReadConfigFile(FileInfo configFile, JsonSerializer jsonSerializer, ILogger<CrunchLog> logger)
        {
            logger.LogDebug($"Read configuration from: {configFile}");

            var config = new CrunchConfig();
            jsonSerializer.Populate(configFile.OpenText(), config);

            logger.LogDebug("Configration read");
            logger.LogInformation($"Output path: {config.Paths.OutputPath}");

            return config;
        }
    }
}
