using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Bit0.CrunchLog.Config
{
    public class Theme
    {
        public Theme(FileInfo themeConfig)
        {
            ConfigFile = themeConfig;
            Directory = themeConfig.Directory;
        }

        [JsonIgnore]
        public FileInfo ConfigFile { get; }

        [JsonIgnore]
        public DirectoryInfo Directory { get; }

        [JsonProperty("outputType")]
        public ThemeOutputType OutputType { get; set; } = ThemeOutputType.Html;

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("version")]
        public String Version { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("homepage")]
        public String Homepage { get; set; }

        [JsonProperty("assets")]
        public Output Output { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<String> Tags { get; set; }

        [JsonProperty("features")]
        public IEnumerable<String> Features { get; set; }

        [JsonProperty("license")]
        public License License { get; set; }


        public static Theme Get(DirectoryInfo themeDir)
        {
            var configFile = themeDir.CombineFilePath(".json", "theme");
            var theme = new Theme(configFile);
            JsonConvert.PopulateObject(configFile.OpenText().ReadToEnd(), theme);

            return theme;
        }

        public static Theme Get(String zipUrl, DirectoryInfo themeDir)
        {
            using (var wc = new WebClient())
            {
                var zipFile = new FileInfo("theme.zip");
                wc.DownloadFile(zipUrl, zipFile.FullName);

                if (zipFile.Exists && !themeDir.CombineFilePath("theme.json").Exists)
                {
                    ZipFile.ExtractToDirectory(zipFile.FullName, themeDir.FullName);
                }
            }

            return Theme.Get(themeDir);
        }
    }
}
