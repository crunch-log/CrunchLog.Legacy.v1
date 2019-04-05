using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class CategoryInfo : IMenuItem
    {
        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("color")]
        public String Color { get; set; }

        public String Permalink { get; set; }

        [JsonProperty("image")]
        public String Image { get; set; }

        [JsonProperty("imageMedium")]
        public String ImageMedium { get; set; }

        [JsonProperty("imageSmall")]
        public String ImageSmall { get; set; }

        [JsonProperty("imagePlaceholder")]
        public String ImagePlaceholder { get; set; }

        [JsonProperty("showInMainMenu")]
        public Boolean ShowInMainMenu { get; set; }

        [JsonProperty("url")]
        public String Url { get => Permalink; set => Permalink = value; }
    }
}
