using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Bit0.CrunchLog.Extensions;
using Bit0.Registry.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bit0.CrunchLog.Config {
    public class Theme : Pack {
        public Theme (FileInfo themeConfig) {
            PackFile = themeConfig;

            TemplateRoot = PackFile.Directory.CombineDirPath ("src");
            if (!TemplateRoot.Exists) {
                TemplateRoot = PackFile.Directory;
            }
        }

        [JsonProperty ("outputType")]
        public ThemeOutputType OutputType { get; set; } = ThemeOutputType.Html;

        [JsonProperty ("output")]
        public ThemeOutput Output { get; set; }

        [JsonProperty ("engineType")]
        public String EngineType { get; set; }

        [JsonIgnore]
        public DirectoryInfo TemplateRoot { get; set; }

        public static Theme Get (FileInfo themeFile, DirectoryInfo outputDir) {
            var theme = new Theme (themeFile);

            using (var streamReader = themeFile.OpenText ()) {
                JsonConvert.PopulateObject (streamReader.ReadToEnd (), theme);
            }

            theme.Output.Data = outputDir.CombineDirPath (theme.Output.Data.Name);

            return theme;
        }
    }
}