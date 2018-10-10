using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Bit0.CrunchLog.Config
{
    public class CrunchSite
    {
        [JsonProperty("baseUrl")]
        public String BaseUrl { get; set; } = "";

        [JsonProperty("languageCode")]
        public String LanguageCode { get; set; } = "en-US";

        [JsonProperty("title")]
        public String Title { get; set; } = "CrunchLog";

        [JsonProperty("subtitle")]
        public String SubTitle { get; set; } = "Static blog generator";

        [JsonProperty("theme")]
        public String ThemeKey { get; set; } = "default";

        [JsonIgnore]
        public Theme Theme { get; set; }

        [JsonProperty("favicon")]
        public String FavIcon { get; set; } = "favicon.ico";

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
        public IDictionary<String, IEnumerable<MenuItem>> Menu { get; set; }

        [JsonProperty("categories")]
        public IDictionary<String, String> Categories { get; set; }

        [JsonProperty("paths")]
        public ConfigPaths Paths { get; set; } = new ConfigPaths();

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; } = new Pagination();

        [JsonProperty("defaultBanner")]
        public String DefaultBannerKey { get; internal set; }

        [JsonIgnore]
        public FileInfo DefaultBanner { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            // load themes directory
            Theme = Theme.Get(Paths.ThemesPath.CombineDirPath(ThemeKey));
            DefaultBanner = ImageHelpers.GetImagePath(DefaultBannerKey, Paths.ContentPath, Paths.ImagesPath, null);
        }
    }
}