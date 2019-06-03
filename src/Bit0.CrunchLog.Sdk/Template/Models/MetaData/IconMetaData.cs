using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models.MetaData
{
    public class IconMetaData
    {
        [JsonProperty("favicon")] public String Favicon { get; set; }
        [JsonProperty("favicon16")] public String Favicon16 { get; set; }
        [JsonProperty("favicon32")] public String Favicon32 { get; set; }
        [JsonProperty("favicon144")] public String Favicon144 { get; set; }
        [JsonProperty("favicon152")] public String Favicon152 { get; set; }
        [JsonProperty("favicon192")] public String Favicon192 { get; set; }
        [JsonProperty("favicon512")] public String Favicon512 { get; set; }
        [JsonProperty("pinSvg")] public String PinSvg { get; set; }

    }
}