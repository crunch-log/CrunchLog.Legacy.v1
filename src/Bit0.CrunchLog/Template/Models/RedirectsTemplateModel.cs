using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bit0.CrunchLog.Template.Models
{
    public class RedirectsTemplateModel : ITemplateModel
    {
        [JsonIgnore]
        public String Title { get; set; }
        [JsonIgnore]
        public String Layout { get; set; } = "site";
        [JsonIgnore]
        public String Permalink { get; set; } = "/";

        [JsonProperty("redirects")]
        public IDictionary<String, String> Redirects { get; set; }
    }
}
