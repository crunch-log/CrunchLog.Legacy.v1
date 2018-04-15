using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
        
        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("homepage")]
        public String Homepage { get; set; }

        [JsonProperty("assets")]
        public Assets Assets { get; set; }
        
        [JsonProperty("tags")]
        public IEnumerable<String> Tags { get; set; }

        [JsonProperty("features")]
        public IEnumerable<String> Features { get; set; }

        [JsonProperty("license")]
        public License License { get; set; }
    }

    public class License
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        
        [JsonProperty("link")]
        public String Link { get; set; }
    }

    public class Assets
    {
        [JsonProperty("dirs")]
        public IEnumerable<String> Directories { get; set; }

        [JsonProperty("files")]
        public IEnumerable<String> Files { get; set; }
    }
}
