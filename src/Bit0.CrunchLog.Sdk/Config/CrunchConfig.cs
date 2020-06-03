using Bit0.CrunchLog.JsonConverters;
using Bit0.Registry.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace Bit0.CrunchLog.Config
{
    public class CrunchConfig
    {
        public CrunchConfig(FileInfo configFile)
        {
            File = configFile;

            // make sure we have defaults
            Paths = new ConfigPaths();
            Paths.SetupPaths(configFile.Directory);

            Theme = new Theme("default");
            Copyright = new Copyright();
            Authors = new Dictionary<String, Author>();
            Menu = new Dictionary<String, IEnumerable<MenuItem>>();
            Categories = new Dictionary<String, CategoryInfo>();
            Tags = new Dictionary<String, CategoryInfo>();
            Pagination = new Pagination();
            PackageSources = new Dictionary<String, String>();
        }

        [JsonIgnore]
        [Description()]
        public FileInfo File { get; }

        [JsonProperty("languageCode")]
        public String LanguageCode { get; set; } = "en-US";

        [JsonProperty("title")]
        public String Title { get; set; } = "CrunchLog";

        [JsonProperty("subtitle")]
        public String SubTitle { get; set; } = "Static blog generator";

        [JsonProperty("baseUrl")]
        public String BaseUrl { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; } = "Static blog generator, written in .NET Core.";

        [JsonProperty("theme")]
        [JsonConverter(typeof(ThemeConverter))]
        public Theme Theme { get; set; }

        [JsonProperty("copyright")]
        public Copyright Copyright { get; set; }

        [JsonProperty("permalink")]
        public String Permalink { get; set; } = @"/:year/:month/:slug";

        [JsonProperty("authors")]
        public IDictionary<String, Author> Authors { get; set; }

        [JsonProperty("menu")]
        public IDictionary<String, IEnumerable<MenuItem>> Menu { get; set; }

        [JsonProperty("categories")]
        public IDictionary<String, CategoryInfo> Categories { get; set; }

        [JsonProperty("defaultCategory")]
        public String DefaultCategory { get; set; }

        [JsonProperty("tags")]
        [JsonConverter(typeof(TagsConverters))]
        public IDictionary<String, CategoryInfo> Tags { get; set; }

        [JsonProperty("paths")]
        public ConfigPaths Paths { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("defaultImage")]
        public SiteImage DefaultBannerImage { get; set; }

        [JsonProperty("packageSources")]
        public IDictionary<String, String> PackageSources { get; }

        [JsonProperty("manifest")]
        public SiteManifest Manifest { get; set; }

        [JsonProperty("icons")]
        public IDictionary<String, SiteImage> Icons { get; set; }

        [JsonProperty("social")]
        public IDictionary<String, String> Social { get; set; }

        [JsonIgnore]
        public IEnumerable<Package> Plugins { get; set; }

        [JsonProperty("robots")]
        public IEnumerable<String> Robots { get; set; } = new[] { "index", "follow" };

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Paths.SetupPaths(File.Directory);
        }
    }
}