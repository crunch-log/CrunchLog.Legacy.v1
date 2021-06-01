using System;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class SiteImage
    {
        [JsonProperty("src")]
        public String Url { get; set; }

        [JsonProperty("size")]
        public String Size => $"{Width}x{Height}";

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("width")]
        public Int32 Width { get; set; }

        [JsonProperty("height")]
        public Int32 Height { get; set; }

        [JsonProperty("placeholder")]
        public String Placeholder { get; set; }
    }
}