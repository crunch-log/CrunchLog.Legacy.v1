using Bit0.CrunchLog.Config;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Template.Models
{
    public class SiteTemplateModel
    {
        public String Title { get; set; }
        public String SubTitle { get; set; }
        public String Owner { get; set; }
        public IDictionary<String, IEnumerable<MenuItem>> Menu { get; set; }
    }
}
