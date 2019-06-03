using Bit0.CrunchLog.Extensions;
using Bit0.Registry.Core;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Bit0.CrunchLog.Config
{
    public class Theme : Pack
    {
        public Theme(FileInfo themeConfig)
        {
            PackFile = themeConfig;
        }

        [JsonProperty("outputType")]
        public ThemeOutputType OutputType { get; set; } = ThemeOutputType.Html;

        [JsonProperty("output")]
        public ThemeOutput Output { get; set; }

        [JsonProperty("engineType")]
        public String EngineType { get; set; }

        public static Theme Get(FileInfo themeFile, DirectoryInfo outputDir)
        {
            if (!themeFile.Exists)
            {
                themeFile = themeFile.Directory.Parent.CombineFilePath(".json", "pack");
            }

            var theme = new Theme(themeFile);

            using (var streamReader = themeFile.OpenText())
            {
                JsonConvert.PopulateObject(streamReader.ReadToEnd(), theme);
            }
            
            theme.Output.Data = outputDir.CombineDirPath(theme.Output.Data.Name);

            return theme;
        }
    }
}
