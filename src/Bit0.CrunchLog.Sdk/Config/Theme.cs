using Bit0.CrunchLog.Extensions;
using Bit0.Registry.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Bit0.CrunchLog.Config
{
    public class Theme : Pack
    {
        public Theme(FileInfo themeConfig)
        {
            PackFile = themeConfig;
        }

        [JsonExtensionData]
        private readonly IDictionary<String, JToken> _additionalData = new Dictionary<String, JToken>();

        [JsonProperty("outputType")]
        public ThemeOutputType OutputType { get; set; } = ThemeOutputType.Html;

        [JsonProperty("output")]
        public ThemeOutput Output { get; set; }

        [JsonProperty("engineType")]
        public String EngineType { get; set; }

        [JsonIgnore]
        public DirectoryInfo TemplateRoot { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var srcKey = (String)_additionalData["sourceDir"];
            TemplateRoot = PackFile.Directory.CombineDirPath(srcKey);
        }

        public static Theme Get(FileInfo themeFile, DirectoryInfo outputDir)
        {
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
