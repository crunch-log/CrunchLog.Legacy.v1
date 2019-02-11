using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class MenuItem
    {
        [JsonProperty("title")]
        public String Title { get; set; }
        [JsonProperty("url")]
        public String Url { get; set; }
        [JsonProperty("order")]
        public Int32 Order { get; set; }
    }
}