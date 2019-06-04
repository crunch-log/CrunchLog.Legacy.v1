using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models.MetaData
{
    public class RedirectMetaData
    {
        [JsonProperty("time")] public Int32 Time { get; set; }
        [JsonProperty("url")] public String Url { get; set; }
    }
}