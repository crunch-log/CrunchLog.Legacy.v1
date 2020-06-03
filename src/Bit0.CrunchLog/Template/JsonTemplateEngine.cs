using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Bit0.CrunchLog.Template
{
    public class JsonTemplateEngine : ITemplateEngine
    {
        private readonly CrunchConfig _siteConfig;

        public JsonTemplateEngine(CrunchConfig siteConfig)
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
            if (model is RedirectsListTemplateModel)
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


        public void PreProcess() { }

        public void PostProcess(CrunchConfig siteConfig, Theme theme)
        {
            ProcessPreCache(siteConfig, theme);
        }

        private void ProcessPreCache(CrunchConfig siteConfig, Theme theme)
        {
            var precache = siteConfig.Paths.ThemesPath
                .GetFiles(theme.Output.Process["precache"], System.IO.SearchOption.TopDirectoryOnly)
                .FirstOrDefault();

            var to = siteConfig.Paths.OutputPath.CombineFilePath("js", precache.Name);

            var indexH = $"  {{\r\n    \"revision\": \"{Guid.NewGuid():N}\",\r\n    \"url\": \"/data/index.json\"\r\n  }}";
            var siteInfoH = $"  {{\r\n    \"revision\": \"{Guid.NewGuid():N}\",\r\n    \"url\": \"/data/siteInfo.json\"\r\n  }}";

            var preContent = precache.ReadText();
            preContent = preContent.Replace("\n];", $",\r\n{indexH},\r\n{siteInfoH}\r\n];");

            to.WriteText(preContent);
        }
    }
}
