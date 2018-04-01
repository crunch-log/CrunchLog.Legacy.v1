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

        [JsonProperty("content")] public String Content { get; set; } = StaticPaths.Content;
        [JsonProperty("themes")] public String Themes { get; set; } = StaticPaths.Themes;
        [JsonProperty("plugins")] public String Plugins { get; set; } = StaticPaths.Plugins;
        [JsonProperty("output")]public String Output { get; set; } = StaticPaths.Output;

        [JsonIgnore]
        public DirectoryInfo BasePath => _configFile.Directory;

        [JsonIgnore]
        public DirectoryInfo OutputPath => new DirectoryInfo(BasePath.CombinePath(Output.NormalizePath()));

    }

    public static class StaticPaths
    {
        public const String ConfigFile = "crunch.json";
        public const String Content = "Content";
        public const String Themes = "Themes";
        public const String Plugins = "Plugins";
        public const String Output = "_site";
    }
}