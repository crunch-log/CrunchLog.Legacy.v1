using Bit0.CrunchLog.Template.Models;
using System;

namespace Bit0.CrunchLog.Template
{
    public interface ITemplateEngine
    {
        void Render(ITemplateModel model);
        void Render(SiteTemplateModel model);
    }
}
