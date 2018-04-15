using System;
using System.IO;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.TemplateModels;
using HandlebarsDotNet;
using HandlebarsDotNet.Compiler.Resolvers;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.ThemeHandler
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HandelbarsThemeHandler : ThemeHandlerBase
    {
        private readonly IHandlebars _handlebars;

        public HandelbarsThemeHandler(CrunchConfig config, JsonSerializer jsonSerializer) : base(config, jsonSerializer)
        {
            var handlebars = Handlebars.Create(new HandlebarsConfiguration
            {
                ExpressionNameResolver = new UpperCamelCaseExpressionNameResolver()
            });

            var hbs = config.Site.Theme.CombineDirPath("shared").GetFiles(".hbs", SearchOption.AllDirectories);

            handlebars.RegisterHelper("alt", (output, context, arguments) =>
            {
                var i = (Int32) arguments[0];
                output.WriteSafeString(i % 2 == 0 ? arguments[1] : arguments[2]);
            });

            _handlebars = handlebars;
        }

        public override void WriteFile(string template, ITemplateModel model)
        {
            var outputDir = Config.Paths.OutputPath.CombineDirPath(model.Permalink.Substring(1));
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }
            
            var file = outputDir.CombineFilePath(".html", "index");

            var templateFile = Config.Site.Theme.CombineFilePath(".hbs", template);
            var handlebarsTemplate = _handlebars.Compile(templateFile.OpenText().ReadToEnd());

            using (var write = file.CreateText())
            {
                var content = handlebarsTemplate(model);
                write.Write(content);
            }
        }
    }
}
