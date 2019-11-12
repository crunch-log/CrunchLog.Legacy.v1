using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class Pagination
    {
        [JsonProperty("pageSize")]
        public Int32 PageSize { get; set; }
    }
}