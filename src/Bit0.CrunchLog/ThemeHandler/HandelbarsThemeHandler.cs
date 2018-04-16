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

            _handlebars = handlebars;

            RegisterHelpers();
            RegisterPartials(config);
        }

        private void RegisterHelpers()
        {
            _handlebars.RegisterHelper("alt", (output, context, arguments) =>
            {
                var i = (Int32) arguments[0];
                output.WriteSafeString(i % 2 == 0 ? arguments[1] : arguments[2]);
            });
        }

        private void RegisterPartials(CrunchConfig config)
        {
            var templates = config.Site.Theme.GetFiles("*.hbs", SearchOption.AllDirectories);
            foreach (var partial in templates)
            {
                var dir = partial.DirectoryName.Replace(config.Site.Theme.FullName, "");
                dir = dir.Replace("\\shared\\", "");

                if (!String.IsNullOrWhiteSpace(dir))
                {
                    dir += "/";
                }

                var name = $"{dir}{Path.GetFileNameWithoutExtension(partial.FullName)}";
                var source = File.ReadAllText(partial.FullName);

                using (var reader = new StringReader (source)) {
                    var partialTemplate = _handlebars.Compile (reader);
                    _handlebars.RegisterTemplate(name, partialTemplate);
                }
            }
        }

        public override void WriteFile(String template, ITemplateModel model)
        {
            var outputDir = Config.Paths.OutputPath.CombineDirPath(model.Permalink.Substring(1));
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }
            
            var file = outputDir.CombineFilePath(".html", "index");

            var handlebarsTemplate = _handlebars.Configuration.RegisteredTemplates[template];

            using (var write = file.CreateText())
            {
                handlebarsTemplate(write, model);
            }
        }
    }
}
