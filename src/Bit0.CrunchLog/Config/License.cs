using System;
using Newtonsoft.Json;

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