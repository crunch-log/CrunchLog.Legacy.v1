using System;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.TemplateModels;
using HandlebarsDotNet;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Theme
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HandelbarsThemeHandler : ThemeHandlerBase
    {
        public HandelbarsThemeHandler(CrunchConfig config, JsonSerializer jsonSerializer) : base(config, jsonSerializer)
        { }

        public override void WriteFile(string template, ITemplateModel model)
        {
            var outputDir = new DirectoryInfo(Config.Paths.OutputPath.CombinePath(model.Permalink.Substring(1)).NormalizePath());
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }
            
            var file = new FileInfo(outputDir.CombinePath("index.html"));

            var templateFile = new FileInfo(Config.Site.Theme.CombinePathEx(".hbs", template));
            var handlebarsTemplate = Handlebars.Compile(templateFile.OpenText().ReadToEnd());

            using (var write = file.CreateText())
            {
                var content = handlebarsTemplate(model);
                write.Write(content);
            }
        }
    }
}
