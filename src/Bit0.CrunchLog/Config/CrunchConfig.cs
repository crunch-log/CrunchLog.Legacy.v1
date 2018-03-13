using System;
using System.Collections.Generic;
using System.IO;
using Bit0.CrunchLog.Extensions;
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

        [JsonProperty("permalinks")]
        public IDictionary<String, String> Permalinks { get; set; } = new Dictionary<String, String>
        {
            {CrunchConfigKeys.Posts, @"/:year/:month/:slug"},
        };


        [JsonProperty("dateformat")]
        public String DateFormat { get; set; } = "yyyy-MM-dd hh:mm";

        [JsonProperty("authors")]
        public IDictionary<String, Author> Authors { get; set; }

        [JsonProperty("menu")]
        public IDictionary<String, IEnumerable<Menu>> Menu { get; set; }

        [JsonProperty("paths")]
        public IDictionary<String, String> Paths { get; set; } = new Dictionary<String, String>
        {
            {CrunchConfigKeys.Posts, @"Contents\Posts\"},
            {CrunchConfigKeys.Pages, @"Contents\Pages\"},
            {CrunchConfigKeys.Output, @"_site\"},
        };

        [JsonIgnore]
        public DirectoryInfo BasePath => _configFile.Directory;

        [JsonIgnore]
        public DirectoryInfo OutputPath => new DirectoryInfo(BasePath.CombinePath(Paths[CrunchConfigKeys.Output].NormalizePath()));

        public CrunchConfig(FileInfo configFile)
        {
            _configFile = configFile;
        }
    }

    public static class CrunchConfigKeys
    {
        public const String Posts = "posts";
        public const String Pages = "pages";
        public const String Output = "output";
    }
}