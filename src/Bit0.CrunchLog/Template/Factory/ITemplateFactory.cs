using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Template.Factory
{
    public interface ITemplateFactory
    {
        CrunchSite SiteConfig { get; }

        Theme Theme { get; }

        ITemplateEngine Engine { get; }

        void PreProcess();

        void PostProcess();
        
        void Render(ITemplateModel model);
    }
}
