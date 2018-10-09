using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Template.Factory
{
    public interface ITemplateFactory
    {
        CrunchSite SiteConfig { get; }

        Theme Theme { get; }

        ITemplateEngine Engine { get; }
        
        void Render(ITemplateModel model);

        void Render(SiteTemplateModel model);
    }
}
