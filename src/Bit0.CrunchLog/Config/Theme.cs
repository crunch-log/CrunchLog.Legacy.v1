using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace Bit0.CrunchLog.Config
{
    public class Theme
    {
        public Theme(FileInfo themeConfig)
        {
            ConfigFile = themeConfig;
        }

        [JsonIgnore]
        public FileInfo ConfigFile { get; }

        [JsonIgnore]
        public DirectoryInfo Directory => ConfigFile.Directory;

        [JsonProperty("outputType")]
        public ThemeOutputType OutputType { get; set; } = ThemeOutputType.Html;

        [JsonProperty("output")]
        public ThemeOutput Output { get; set; }

        public static Theme Get(DirectoryInfo themeDir, DirectoryInfo outputDir)
        {
            var configFile = themeDir.CombineFilePath(".json", "theme");
            var theme = new Theme(configFile);

            using (var streamReader = configFile.OpenText())
            {
                JsonConvert.PopulateObject(streamReader.ReadToEnd(), theme);
            }
            
            theme.Output.Data = outputDir.CombineDirPath(theme.Output.Data.Name);

            return theme;
        }

        public static Theme Get(String zipUrl, DirectoryInfo themeDir, DirectoryInfo outputDir)
        {
            if (themeDir.Exists && themeDir.CombineFilePath("theme.json").Exists)
            {
                Task.Run(() => { themeDir.ClearFolder(); });
            }

            using (var wc = new WebClient())
            {
                var zipFile = new FileInfo($"theme{DateTime.Now.ToBinary().ToString()}.zip");
                wc.DownloadFile(zipUrl, zipFile.FullName);

                if (zipFile.Exists)
                {
                    ZipFile.ExtractToDirectory(zipFile.FullName, themeDir.FullName);
                }

                zipFile.Delete();
            }

            return Theme.Get(themeDir, outputDir);
        }
    }
}
