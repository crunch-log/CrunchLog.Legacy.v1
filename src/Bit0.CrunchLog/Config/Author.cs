using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Author
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("homepage")]
        public String HomePage { get; set; }
        [JsonProperty("social")]
        public IDictionary<String, String> Social { get; set; }

        public override String ToString()
        {
            return Name;
        }
    }
}