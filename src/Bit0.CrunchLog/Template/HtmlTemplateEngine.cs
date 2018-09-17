using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Template
{
    public class HtmlTemplateEngine : IHtmlTemplateEngine
    {
        private readonly CrunchSite _siteConfig;

        public HtmlTemplateEngine(CrunchSite siteConfig)
        {
            _siteConfig = siteConfig;
        }

        public void Render(ITemplateModel model)
        {
            var outputDir = _siteConfig.Paths.OutputPath.CombineDirPath(model.Permalink.Replace("//", "/").Substring(1));
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }

            var file = outputDir.CombineFilePath(".json", "index");

            using (var write = file.CreateText())
            {

            }
        }
    }
}
