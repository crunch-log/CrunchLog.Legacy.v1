using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class CrunchConfig
    {
        private readonly FileInfo _configFile;

        [JsonProperty("site")]
        public Site Site { get; set; } = new Site();

        [JsonProperty("copyright")]
        public Copyright Copyright { get; set; } = new Copyright();

        [JsonProperty("tags")]
        public IEnumerable<String> Tags { get; set; } = new List<String>();

        [JsonProperty("permalink")]
        public String Permalink { get; set; } = @"/:year/:month/:slug";

        [JsonProperty("dateformat")]
        public String DateFormat { get; set; } = "yyyy-MM-dd hh:mm";

        [JsonProperty("authors")]
        public IDictionary<String, Author> Authors { get; set; }

        [JsonProperty("menu")]
        public IDictionary<String, IEnumerable<Menu>> Menu { get; set; }

        [JsonProperty("paths")]
        public ConfigPaths Paths { get; set; }

        public CrunchConfig(FileInfo configFile)
        {
            _configFile = configFile;

            Paths = new ConfigPaths(_configFile);
        }
    }
}