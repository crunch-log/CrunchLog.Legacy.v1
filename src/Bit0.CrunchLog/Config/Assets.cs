using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Output
    {
        [JsonProperty("content")]
        public FileInfo Content { get; set; }

        [JsonProperty("copy")]
        public IDictionary<String, IEnumerable<String>> Copy { get; set; }

        [JsonProperty("process")]
        public IDictionary<String, String> Process { get; set; }
    }
}