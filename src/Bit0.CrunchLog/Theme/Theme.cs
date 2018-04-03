using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Theme
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

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("author")]
        public String Author { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("assets")]
        public Assets Assets { get; set; }
    }

    public class Assets
    {
        [JsonProperty("dirs")]
        public IEnumerable<String> Directories { get; set; }

        [JsonProperty("files")]
        public IEnumerable<String> Files { get; set; }
    }
}
