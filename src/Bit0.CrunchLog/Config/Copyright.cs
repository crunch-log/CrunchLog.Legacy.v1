using System;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Copyright
    {
        [JsonProperty("startYear")]
        public Int32 StartYear { get; set; } = 2018;

        [JsonProperty("owner")]
        public String Owner { get; set; } = "Nullbit";
    }
}