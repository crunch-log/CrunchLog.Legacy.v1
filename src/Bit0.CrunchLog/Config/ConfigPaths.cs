using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;

namespace Bit0.CrunchLog.Config
{
    public class ConfigPaths
    {
        public ConfigPaths()
        {
            BasePath = ServiceProviderFactory.Current.GetService<ConfigFile>().File.Directory;
            
            ContentPath = BasePath.CombineDirPath(StaticPaths.Content.NormalizePath());
            ThemesPath = BasePath.CombineDirPath(StaticPaths.Themes.NormalizePath());
            PluginsPath = BasePath.CombineDirPath(StaticPaths.Plugins.NormalizePath());
            OutputPath = BasePath.CombineDirPath(StaticPaths.Output.NormalizePath());
            ImagesPath = BasePath.CombineDirPath(StaticPaths.Images.NormalizePath());
            AssetsPath = BasePath.CombineDirPath(StaticPaths.Assets.NormalizePath());
        }

        [JsonIgnore]
        public DirectoryInfo BasePath { get; }
                
        [JsonProperty("content")]
        [JsonConverter(typeof(PathConverter), StaticPaths.Content)]
        public DirectoryInfo ContentPath { get; set; }

        [JsonProperty("themes")]
        [JsonConverter(typeof(PathConverter), StaticPaths.Themes)]
        public DirectoryInfo ThemesPath { get; set; } 

        [JsonProperty("plugins")]
        [JsonConverter(typeof(PathConverter), StaticPaths.Plugins)]
        public DirectoryInfo PluginsPath { get; set; }

        [JsonProperty("output")]
        [JsonConverter(typeof(PathConverter), StaticPaths.Output)]
        public DirectoryInfo OutputPath { get; set; }

        [JsonProperty("images")]
        [JsonConverter(typeof(PathConverter), StaticPaths.Images)]
        public DirectoryInfo ImagesPath { get; set; }

        [JsonProperty("assets")]
        [JsonConverter(typeof(PathConverter), StaticPaths.Assets)]
        public DirectoryInfo AssetsPath { get; set; }
    }
}