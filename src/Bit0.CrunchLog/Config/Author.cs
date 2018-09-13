using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Author
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("alias")]
        public String Alias { get; set; }
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("homepage")]
        public String HomePage { get; set; }
        [JsonProperty("social")]
        public IDictionary<String, String> Social { get; set; }

        [JsonIgnore]
        public String Permalink => $"/by/{Alias}";

        public override String ToString()
        {
            return $"{Name} ({Alias})";
        }
    }
}