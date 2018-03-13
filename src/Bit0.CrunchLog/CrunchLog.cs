using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Extensions;

namespace Bit0.CrunchLog
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CrunchLog
    {
        private readonly DirectoryInfo _basePath;
        private readonly JsonSerializer _jsonSerializer;
        private readonly ILogger<CrunchLog> _logger;
        
        public CrunchConfig Config { get; }

        public CrunchLog(Arguments arguments,JsonSerializer jsonSerializer, ILogger<CrunchLog> logger)
        {
            _jsonSerializer = jsonSerializer;
            _logger = logger;

            _basePath = new DirectoryInfo(arguments.BasePath);
            _logger.LogInformation($"Base path: {_basePath}");

            Config = ReadConfigFile();
        }

        private CrunchConfig ReadConfigFile()
        {
            var configFile = _basePath.GetFiles("crunch.json", SearchOption.TopDirectoryOnly).SingleOrDefault();

            if (configFile == null)
            {
                var errorMsg = $"Cannot find {_basePath.CombinePath("crunch.json")}";
                throw new FileNotFoundException(errorMsg);
            }

            _logger.LogDebug($"Read configuration from: {configFile}");

            var config = new CrunchConfig(configFile);
            _jsonSerializer.Populate(configFile.OpenText(), config);

            _logger.LogDebug("Configration read");
            _logger.LogInformation($"Output path: {config.OutputPath}");
            
            return config;
        }
    }
}
