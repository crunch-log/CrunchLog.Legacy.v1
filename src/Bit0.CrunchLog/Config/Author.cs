using Bit0.CrunchLog.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        [JsonProperty("url")]
        public String Permalink => String.Format(StaticKeys.ByPathFormat, Alias);

        public override String ToString()
        {
            return $"{Name} ({Alias})";
        }
    }
}