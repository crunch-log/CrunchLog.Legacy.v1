using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class CategoryInfo
    {
        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("color")]
        public String Color { get; set; }

        [JsonProperty("url")]
        public String Permalink { get; set; }

        [JsonProperty("image")]
        public String Image { get; set; }

        [JsonProperty("thumb")]
        public String Thumbnail { get; set; }

        [JsonProperty("thumbSmall")]
        public String ThumbnailSmall { get; set; }

        [JsonProperty("showInMainMenu")]
        public Boolean ShowInMainMenu { get; set; }

    }
}
