using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Template.Models
{
    public class SiteTemplateModel : ITemplateModel
    {
        [JsonProperty("title")]
        public String Title { get; set; }
        [JsonIgnore]
        public String Layout { get; set; } = Layouts.Site.GetValue();
        [JsonProperty("url")]
        public String Permalink { get; set; } = "/";
        [JsonProperty("subTitle")]
        public String SubTitle { get; set; }
        [JsonProperty("owner")]
        public String Owner { get; set; }
        [JsonProperty("copyright")]
        public Int32 CopyrightYear { get; set; }
        [JsonProperty("menu")]
        public IDictionary<String, IEnumerable<MenuItem>> Menu { get; set; }
        [JsonProperty("categories")]
        public IDictionary<String, CategoryInfo> Categories { get; set; }
        [JsonProperty("tags")]
        public IEnumerable<TagMenuItem> Tags { get; set; }
        [JsonProperty("authors")]
        public IDictionary<String, Author> Authors { get; set; }
    }
}
