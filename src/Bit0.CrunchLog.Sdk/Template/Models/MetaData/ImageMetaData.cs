using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models.MetaData
{
    public class ImageMetaData
    {
        [JsonProperty("text")] public String Type { get; set; }
        [JsonProperty("url")] public String Url { get; set; }
        [JsonProperty("width")] public Int32 Width { get; set; }
        [JsonProperty("height")] public Int32 Height { get; set; }
    }
}