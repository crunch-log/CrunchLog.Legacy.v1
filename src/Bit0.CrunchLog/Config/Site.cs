using System;
using System.IO;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Site
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
        public DirectoryInfo Theme { get; set; }

        [JsonProperty("favicon")]
        public String FavIcon { get; set; } = "favicon.ico";
    }
}