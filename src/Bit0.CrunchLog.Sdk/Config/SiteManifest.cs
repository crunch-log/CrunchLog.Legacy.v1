using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class SiteManifest
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("shortName")]
        public String ShortName { get; set; }
        [JsonProperty("startUrl")]
        public String StartUrl { get; set; }
        [JsonProperty("display")]
        public String Display { get; set; }
        [JsonProperty("backgroundColor")]
        public String BackgroundColor { get; set; }
        [JsonProperty("themeColor")]
        public String ThemeColor { get; set; }
    }

}