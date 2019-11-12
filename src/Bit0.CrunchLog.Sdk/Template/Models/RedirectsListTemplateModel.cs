using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Template.Models
{
    public class RedirectsListTemplateModel : ITemplateModel
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
