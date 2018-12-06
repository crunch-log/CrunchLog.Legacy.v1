using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Bit0.CrunchLog.Template
{
    public class JsonTemplateEngine : ITemplateEngine
    {
        private readonly CrunchSite _siteConfig;

        public JsonTemplateEngine(CrunchSite siteConfig)
        {
            _siteConfig = siteConfig;
        }

        public void Render(ITemplateModel model)
        {
            var outputDir = _siteConfig.Paths.OutputPath;
            if (_siteConfig.Theme.OutputType == ThemeOutputType.Json)
            {
                outputDir = _siteConfig.Theme.Output.Data;
            }

            if (model is SiteTemplateModel)
            {
                Render(model, outputDir, "siteInfo");
            }
            if (model is RedirectsTemplateModel)
            {
                Render(model, outputDir, "redirects");
            }
            if (model is PostTemplateModel m && m.IsDraft)
            {
                outputDir = outputDir.CombineDirPath("draft", m.Id);
                Render(model, outputDir, "index");
            }
            else
            {
                outputDir = outputDir.CombineDirPath(model.Permalink.Replace("//", "/").Substring(1));
                Render(model, outputDir, "index");
            }

        }

        private static void Render<T>(T model, DirectoryInfo outputDir, String name) where T : class
        {
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }

            var file = outputDir.CombineFilePath(".json", name);

            using (var sw = file.CreateText())
            {
                sw.Write(JsonConvert.SerializeObject(model));
            }
        }
    }
}
