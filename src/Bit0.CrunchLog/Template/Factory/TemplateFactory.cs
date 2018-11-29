using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Template.Factory
{
    public class TemplateFactory : ITemplateFactory
    {
        public CrunchSite SiteConfig { get; }

        public Theme Theme { get; }

        public ITemplateEngine Engine { get; }

        public TemplateFactory(
            CrunchSite siteConfig,
            ITemplateEngine templateEngine
            )
        {
            SiteConfig = siteConfig;
            Theme = SiteConfig.Theme;
            Engine = templateEngine;

            if (!SiteConfig.Paths.OutputPath.Exists)
            {
                SiteConfig.Paths.OutputPath.Create();
            }
        }

        public void Render(ITemplateModel model) => Engine.Render(model);

        public void Render(SiteTemplateModel model) => Engine.Render(model);
    }
}
