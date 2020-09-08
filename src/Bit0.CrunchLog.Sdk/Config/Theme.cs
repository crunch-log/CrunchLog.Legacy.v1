using Bit0.CrunchLog.Extensions;
using Bit0.Registry.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bit0.CrunchLog.Config
{
    public class Theme : Pack
    {

        public Theme(String themeName)
        {
            Name = themeName;
            OriginalName = Name;

            var match = Regex.Match(Name, @"^(.*)\|(.*)$");
            if (match.Success)
            {
                Name = match.Groups[1].Value;
                DownloadUrl = match.Groups[2].Value;
            }

            match = Regex.Match(Name, @"^github:[^\/]+\/([^\@]+)\@?.*?$");
            if (match.Success)
            {
                DownloadUrl = Name;
                Name = match.Groups[1].Value;
            }

            if (Regex.IsMatch(Name, @"^http:|https:"))
            {
                DownloadUrl = Name;
                Name = Name.GetSha1Hash();
            }
        }

        public Theme(FileInfo themeConfig)
        {
            PackFile = themeConfig;

            TemplateRoot = PackFile.Directory.CombineDirPath("src");
            if (!TemplateRoot.Exists)
            {
                TemplateRoot = PackFile.Directory;
            }
        }

        [JsonIgnore]
        public String OriginalName { get; }

        [JsonIgnore]
        public String DownloadUrl { get; }

        [JsonProperty("outputType")]
        public ThemeOutputType OutputType { get; set; } = ThemeOutputType.Html;

        [JsonProperty("output")]
        public ThemeOutput Output { get; set; }

        [JsonProperty("engineType")]
        public String EngineType { get; set; }

        [JsonIgnore]
        public DirectoryInfo TemplateRoot { get; set; }

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