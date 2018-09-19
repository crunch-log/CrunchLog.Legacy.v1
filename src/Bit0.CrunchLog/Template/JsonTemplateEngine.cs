using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Bit0.CrunchLog.Template
{
    public class JsonTemplateEngine : IJsonTemplateEngine
    {
        private readonly CrunchSite _siteConfig;

        public JsonTemplateEngine(CrunchSite siteConfig)
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
            using (var sw = file.CreateText())
            {
                sw.Write(JsonConvert.SerializeObject(model));
            }
        }
    }
}
