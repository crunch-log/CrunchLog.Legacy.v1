using System;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.TemplateModels
{
    public interface ITemplateModel
    {
        CrunchConfig Config { get; set; }
        String Permalink { get; set; }
        String Layout { get; }
        String Title { get; }
    }
}
