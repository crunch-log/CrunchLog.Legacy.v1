using System;

namespace Bit0.CrunchLog.Template.Models
{
    public interface ITemplateModel
    {
        SiteTemplateModel Site { get; set; }
        String Permalink { get; set; }
        String Layout { get; }
        String Title { get; }
    }
}
