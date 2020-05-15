using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace Bit0.CrunchLog
{
    public class ConfigFile : IConfigFile
    {
        private static IConfigFile _instance;
        public FileInfo File { get; }
        public DirectoryInfo Directory { get; }
        public JObject JsonObject { get; }
        public ConfigPaths Paths { get; }

        public static IConfigFile Current
        {
            get
            {
                if (_instance == null)
                {
                    throw new FileLoadException("Cannot find an instance");
                }

                return _instance;
            }
        }

        public ConfigFile(String basePath, JsonSerializer jsonSerializer)
        {
            Directory = new DirectoryInfo(basePath);
            var configFile = Directory.GetFiles(StaticPaths.ConfigFile, SearchOption.TopDirectoryOnly).SingleOrDefault();

            if (configFile == null)
            {
                var errorMsg = $"Cannot find {Directory.CombineDirPath(StaticPaths.ConfigFile)}";
                throw new FileNotFoundException(errorMsg);
            }

            File = configFile;
            JsonObject = JObject.Parse(File.ReadText());

            var paths = new ConfigPaths(this);
            jsonSerializer.Populate(JsonObject["paths"].CreateReader(), paths);

            Paths = paths;
        }

        public static IConfigFile Load(String basePath, JsonSerializer jsonSerializer)
        {
            _instance = new ConfigFile(basePath, jsonSerializer);
            return _instance;
        }
    }
}
