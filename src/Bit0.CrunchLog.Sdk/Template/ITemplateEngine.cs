using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Template
{
    public interface ITemplateEngine
    {
        void PreProcess();
        void PostProcess(CrunchConfig siteConfig, Theme theme);
        void Render(ITemplateModel model);
    }
}
