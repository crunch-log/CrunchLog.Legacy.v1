using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Bit0.CrunchLog.Config
{
    public class ConfigPaths
    {
        public ConfigPaths(IConfigFile configFile)
        {
            BasePath = configFile.Directory;
        }
        
        [JsonExtensionData]
        private readonly IDictionary<String, JToken> _additionalData = new Dictionary<String, JToken>();

        [JsonIgnore]
        public DirectoryInfo BasePath { get; }
                
        [JsonIgnore]
        public DirectoryInfo ContentPath { get; set; }

        [JsonIgnore]
        public DirectoryInfo ThemesPath { get; set; }

        [JsonIgnore]
        public DirectoryInfo PluginsPath { get; set; }

        [JsonIgnore]
        public DirectoryInfo OutputPath { get; set; }

        [JsonIgnore]
        public DirectoryInfo ImagesPath { get; set; }

        [JsonIgnore]
        public DirectoryInfo AssetsPath { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            ContentPath = GetPath("content", StaticPaths.Content);
            ThemesPath = GetPath("themes", StaticPaths.Themes);
            PluginsPath = GetPath("plugins", StaticPaths.Plugins);
            OutputPath = GetPath("output", StaticPaths.Output);
            ImagesPath = GetPath("images", StaticPaths.Images);
            AssetsPath = GetPath("assets", StaticPaths.Assets);
        }

        private DirectoryInfo GetPath(String name, String fallback)
        {
            var key = _additionalData.ContainsKey(name) ? (String)_additionalData[name] : String.Empty;
            var path = !String.IsNullOrWhiteSpace(key) ? key : fallback;

            return BasePath.CombineDirPath(path.NormalizePath());
        }
    }
}