using System;

namespace Bit0.CrunchLog.TemplateModels
{
    public interface ITemplateModel
    {
        String Permalink { get; }
        String Layout { get; }
    }
}
