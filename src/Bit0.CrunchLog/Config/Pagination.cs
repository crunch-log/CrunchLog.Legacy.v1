using System;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Config
{
    public class Pagination
    {
        [JsonProperty("pageSize")]
        public Int32 PageSize { get; set; }
    }
}