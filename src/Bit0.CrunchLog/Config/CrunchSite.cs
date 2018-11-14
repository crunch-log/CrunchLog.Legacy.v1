using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
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
        [JsonExtensionData]
        private readonly IDictionary<String, JToken> _additionalData = new Dictionary<String, JToken>();

        [JsonProperty("baseUrl")]
        public String BaseUrl { get; set; } = "";

        [JsonProperty("languageCode")]
        public String LanguageCode { get; set; } = "en-US";

        [JsonProperty("title")]
        public String Title { get; set; } = "CrunchLog";

        [JsonProperty("subtitle")]
        public String SubTitle { get; set; } = "Static blog generator";

        [JsonIgnore]
        public Theme Theme { get; set; }

        [JsonProperty("favicon")]
        public String FavIcon { get; set; } = "favicon.ico";

        [JsonProperty("copyright")]
        public Copyright Copyright { get; set; } = new Copyright();

        [JsonProperty("permalink")]
        public String Permalink { get; set; } = @"/:year/:month/:slug";

        [JsonProperty("dateformat")]
        public String DateFormat { get; set; } = "yyyy-MM-dd hh:mm";

        [JsonProperty("authors")]
        public IDictionary<String, Author> Authors { get; set; }

        [JsonProperty("menu")]
        public IDictionary<String, IEnumerable<MenuItem>> Menu { get; set; }

        [JsonProperty("categories")]
        public IDictionary<String, CategoryInfo> Categories { get; set; }

        [JsonIgnore]
        public IDictionary<String, CategoryInfo> Tags { get; set; }

        [JsonProperty("paths")]
        public ConfigPaths Paths { get; set; } = new ConfigPaths();

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; } = new Pagination();

        [JsonProperty("defaultBanner")]
        public String DefaultBanner { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Categories = Categories.ToDictionary(k => k.Key, v => new CategoryInfo
            {
                Title = v.Key,
                Permalink = String.Format(StaticKeys.CategoryPathFormat, v.Key),
                Color = v.Value.Color,
                Image = v.Value.Image,
            });

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

            var themeKey = (String)_additionalData["theme"];
            Theme = Theme.Get(Paths.ThemesPath.CombineDirPath(themeKey));
        }
    }
}