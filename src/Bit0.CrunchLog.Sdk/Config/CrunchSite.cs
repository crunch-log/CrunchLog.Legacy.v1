using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
using Bit0.Registry.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Bit0.CrunchLog.Config
{
    public class CrunchSite
    {
        public CrunchSite(ConfigPaths paths, ILogger<CrunchSite> logger)
        {
            Paths = paths;
            _logger = logger;
        }

        [JsonExtensionData]
        private readonly IDictionary<String, JToken> _additionalData = new Dictionary<String, JToken>();
        
        private readonly ILogger<CrunchSite> _logger;

        [JsonProperty("languageCode")]
        public String LanguageCode { get; set; } = "en-US";

        [JsonProperty("title")]
        public String Title { get; set; } = "CrunchLog";

        [JsonProperty("subtitle")]
        public String SubTitle { get; set; } = "Static blog generator";

        [JsonIgnore]
        public Theme Theme { get; set; }

        [JsonProperty("copyright")]
        public Copyright Copyright { get; set; } = new Copyright();

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

        [JsonIgnore]
        public IDictionary<String, CategoryInfo> Tags { get; set; }

        [JsonIgnore]
        public ConfigPaths Paths { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; } = new Pagination();

        [JsonProperty("defaultImage")]
        public String DefaultBanner { get; set; }

        [JsonProperty("defaultImagePlaceholder")]
        public String DefaultImagePlaceholder { get; internal set; }

        [JsonProperty("packageSources")]
        public IDictionary<String, String> PackageSources { get; private set; } 

        [JsonIgnore]
        public IEnumerable<Package> Plugins { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            SetupCategories();
            SetupTags();
            SetupPackageFeeds();
            SetupTheme();

            //Plugins
        }

        private void SetupTheme()
        {
            var themeKey = (String)_additionalData["theme"];
            Theme = Theme.Get(Paths.ThemesPath.CombineDirPath(themeKey).CombineFilePath(".json", "pack"), Paths.OutputPath);

            _logger.LogDebug($"Loaded theme({Theme.Name}).");
        }

        private void SetupTags()
        {
            var tags = _additionalData["tags"];
            Tags = tags.ToObject<IEnumerable<String>>()
                .ToDictionary(k => k, v =>
                {
                    return new CategoryInfo
                    {
                        Title = v,
                        Permalink = String.Format(StaticKeys.TagPathFormat, v)
                    };
                });

            _logger.LogTrace($"Read Tags.");
        }

        private void SetupCategories()
        {
            Categories = Categories.ToDictionary(k => k.Key, v =>
                {
                    var cat = v.Value;

                    cat.Title = v.Key;
                    cat.Permalink = String.Format(StaticKeys.CategoryPathFormat, v.Key);

                    return cat;
                });

            _logger.LogTrace($"Read Categories.");
        }

        private void SetupPackageFeeds()
        {
            var defaultSources = new Dictionary<String, String>
            {
                { "default", "https://packages.0labs.se/crunchlog/index.json" }
            };
            PackageSources = defaultSources.Concat(PackageSources).ToDictionary(k => k.Key, v => v.Value);

            _logger.LogDebug($"Read package feeds.");
        }
    }
}