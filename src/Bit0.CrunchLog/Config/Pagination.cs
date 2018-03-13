using System;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Pagination
    {
        [JsonProperty("enable")]
        public Boolean Enable { get; set; }

        [JsonProperty("pageSize")]
        public Int32 PageSize { get; set; }
    }
}