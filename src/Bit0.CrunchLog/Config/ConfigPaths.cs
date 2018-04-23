using System;
using System.IO;
using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class ConfigPaths
    {
        
        private readonly FileInfo _configFile;

        public ConfigPaths(FileInfo configFile)
        {
            _configFile = configFile;
        }

        [JsonProperty("content")] public String ContentPathKey { get; set; } = StaticPaths.Content;
        [JsonProperty("themes")] public String ThemesPathKey { get; set; } = StaticPaths.Themes;
        [JsonProperty("plugins")] public String PluginsPathKey { get; set; } = StaticPaths.Plugins;
        [JsonProperty("output")] public String OutputPathKey { get; set; } = StaticPaths.Output;
        [JsonProperty("images")] public String ImagesPathKey { get; set; } = StaticPaths.Images;

        [JsonIgnore]
        public DirectoryInfo BasePath => _configFile.Directory;

        [JsonIgnore]
        public DirectoryInfo OutputPath => BasePath.CombineDirPath(OutputPathKey.NormalizePath());

        [JsonIgnore]
        public DirectoryInfo ContentPath => BasePath.CombineDirPath(ContentPathKey.NormalizePath());
    
        [JsonIgnore]
        public DirectoryInfo ThemesPath => BasePath.CombineDirPath(ThemesPathKey.NormalizePath());
    
        [JsonIgnore]
        public DirectoryInfo PluginsPath => BasePath.CombineDirPath(PluginsPathKey.NormalizePath());

        [JsonIgnore]
        public DirectoryInfo ImagesPath => BasePath.CombineDirPath(ImagesPathKey.NormalizePath());
    }
}