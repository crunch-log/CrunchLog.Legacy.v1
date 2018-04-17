using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            RegisterPartials();
        }

        private void RegisterHelpers()
        {
            _handlebars.RegisterHelper("alt", (output, context, args) =>
            {
                var i = (Int32)args[0];
                output.WriteSafeString(i % 2 == 0 ? args[1] : args[2]);
            });

            _handlebars.RegisterHelper("format", (output, context, args) =>
            {
                if (args[0] is DateTime date)
                {
                    output.WriteSafeString(date.ToString(args[1].ToString()));
                }
            });

            _handlebars.RegisterHelper("sep", (output, options, context, args) =>
            {
                if (args[0] is Boolean isLast && !isLast)
                {
                    options.Template(output, context);
                    return;
                }

                options.Inverse(output, context);
            });

            _handlebars.RegisterHelper("partial", (output, options, context, args) =>
            {
                if (args[0] is String template && _handlebars.Configuration.RegisteredTemplates.ContainsKey(template))
                {
                    var handlebarsTemplate = _handlebars.Configuration.RegisteredTemplates[template];
                    handlebarsTemplate(output, context);
                }

                options.Inverse(output, context);
            });
        }

        private void RegisterPartials()
        {
            RegisterPartials("shared");
            RegisterPartials("layouts");
        }

        private void RegisterPartials(String subDir)
        {
            var dirPath = Theme.Directory.CombineDirPath(subDir);
            var templates = dirPath.GetFiles("*.hbs", SearchOption.AllDirectories);
            foreach (var partial in templates)
            {
                //var dir = partial.DirectoryName.Replace(Theme.Directory.FullName, "")
                //    .Replace("\\shared", "")
                //    .Replace("\\layouts", "")
                //    .Replace("\\_layouts", "layouts");

                var dir = partial.DirectoryName.Replace(dirPath.FullName, "");

                if (dir.StartsWith(@"\"))
                {
                    dir = dir.Substring(1);
                }

                if (!String.IsNullOrWhiteSpace(dir))
                {
                    dir += "/";
                }

                var name = $"{dir}{Path.GetFileNameWithoutExtension(partial.FullName)}";
                var source = File.ReadAllText(partial.FullName);

                using (var reader = new StringReader(source))
                {
                    var partialTemplate = _handlebars.Compile(reader);
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
