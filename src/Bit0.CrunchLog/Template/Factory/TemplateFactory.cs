using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Bit0.CrunchLog.Template.Factory
{
    public class TemplateFactory : ITemplateFactory
    {
        public CrunchSite SiteConfig { get; }

        public Theme Theme { get; }

        public ITemplateEngine Engine { get; }

        public TemplateFactory(
            CrunchSite siteConfig,
            IHtmlTemplateEngine htmlTemplateEngine,
            IJsonTemplateEngine jsonTemplateEngine
            )
        {
            SiteConfig = siteConfig;
            Theme = SiteConfig.Theme;

            Engine = Theme.OutputType == ThemeOutputType.Html ? htmlTemplateEngine : (ITemplateEngine) jsonTemplateEngine;
        }

        private void InitOutput ()
        {
            if (!SiteConfig.Paths.OutputPath.Exists)
            {
                SiteConfig.Paths.OutputPath.Create();
            }

            foreach (var dir in Theme.Assets.Directories.Select(d => Theme.Directory.CombineDirPath(d)))
            {
                dir.Copy(SiteConfig.Paths.OutputPath.CombineDirPath(dir.Name));
            }

            foreach (var file in Theme.Assets.Files.Select(f => Theme.Directory.CombineFilePath(f)))
            {
                file.CopyTo(SiteConfig.Paths.OutputPath.CombineDirPath(file.Name).FullName, true);
            }
        }

        public void Render(ITemplateModel model) => Engine.Render(model);
    }
}
