using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bit0.CrunchLog.Config
{
    public class ConfigPaths
    {
        [JsonExtensionData]
        private readonly IDictionary<String, JToken> _additionalData = new Dictionary<String, JToken>();

        [JsonIgnore]
        public DirectoryInfo BasePath { get; private set; }

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

        internal void SetupPaths(DirectoryInfo basePath)
        {
            BasePath = basePath;

            ContentPath = GetPath("content", StaticPaths.Content);
            ThemesPath = GetPath("themes", StaticPaths.Themes);
            PluginsPath = GetPath("plugins", StaticPaths.Plugins);
            OutputPath = GetPath("output", StaticPaths.Output);
            ImagesPath = GetPath("images", StaticPaths.Images);
            AssetsPath = GetPath("assets", StaticPaths.Assets);
        }

        private DirectoryInfo GetPath(String name, String fallback)
        {
            var path = fallback;

            if (_additionalData != null && _additionalData.ContainsKey(name))
            {
                path = (String)_additionalData[name];
            }

            return BasePath.CombineDirPath(path.NormalizePath());
        }
    }
}