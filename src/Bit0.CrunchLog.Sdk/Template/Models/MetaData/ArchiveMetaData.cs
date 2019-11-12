using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models.MetaData
{
    public class ArchiveMetaData
    {
        [JsonProperty("text")] public String Text { get; set; }
        [JsonProperty("url")] public String Url { get; set; }
    }
}