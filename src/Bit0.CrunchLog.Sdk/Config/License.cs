using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class License
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        
        [JsonProperty("link")]
        public String Link { get; set; }
    }
}