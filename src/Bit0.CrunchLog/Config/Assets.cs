using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Assets
    {
        [JsonProperty("dirs")]
        public IEnumerable<String> Directories { get; set; }

        [JsonProperty("files")]
        public IEnumerable<String> Files { get; set; }
    }
}