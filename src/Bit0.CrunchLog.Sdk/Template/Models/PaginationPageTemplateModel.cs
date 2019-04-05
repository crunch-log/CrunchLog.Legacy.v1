using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models
{
    public class PaginationPageTemplateModel
    {
        [JsonProperty("page")]
        public Int32 Page {get; set; }
        [JsonProperty("url")]
        public String Url { get; set; }
        [JsonProperty("isCurrentPage")]
        public Boolean IsCurrentPage {get; set;}

        public override String ToString() => Url;
    }
}
