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
    public class HandelbarsThemeHandler : ThemeHandlerBase
    {
        private readonly IHandlebars _handlebars;

        public HandelbarsThemeHandler(CrunchSite config, JsonSerializer jsonSerializer) : base(config, jsonSerializer)
        {
            var handlebars = Handlebars.Create(new HandlebarsConfiguration
            {
                ExpressionNameResolver = new UpperCamelCaseExpressionNameResolver()
            });

            _handlebars = handlebars;

            RegisterHelpers();
            RegisterTemplates();
        }

        private void RegisterHelpers()
        {
            _handlebars.RegisterHelper("alt", (output, context, args) =>
            {
                if (args[0] is Int32 i)
                {
                    output.WriteSafeString(i % 2 == 0 ? args[1] : args[2]);
                }
                else if (args[0] is Boolean b)
                {
                    output.WriteSafeString(b ? args[1] : args[2]);
                }
            });

            _handlebars.RegisterHelper("format", (output, context, args) =>
            {
                if (args[0] is DateTime date)
                {
                    output.WriteSafeString(date.ToString(args[1].ToString()));
                }
            });

            _handlebars.RegisterHelper("partial", (output, options, context, args) =>
            {
                if (args[0] is String template && _handlebars.Configuration.RegisteredTemplates.ContainsKey(template))
                {
                    var handlebarsTemplate = _handlebars.Configuration.RegisteredTemplates[template];
                    handlebarsTemplate(output, context);
                    return;
                }

                options.Inverse(output, context);
            });

            _handlebars.RegisterHelper("times", (output, options, context, args) =>
            {
                if (args[0] is String s && Int32.TryParse(s, out var n))
                {
                    for (var i = 0; i < n; i++)
                    {
                        options.Template(output, context);
                    }
                    return;
                }

                options.Inverse(output, context);
            });

            _handlebars.RegisterHelper("ifCond", (writer, options, context, args) =>
            {
                if (args.Length != 3)
                {
                    writer.Write("ifCond:Wrong number of arguments");
                    return;
                }
                if (args[0] == null || args[0].GetType().Name == "UndefinedBindingResult")
                {
                    writer.Write("ifCond:args[0] undefined");
                    return;
                }
                if (args[1] == null || args[1].GetType().Name == "UndefinedBindingResult")
                {
                    writer.Write("ifCond:args[1] undefined");
                    return;
                }
                if (args[2] == null || args[2].GetType().Name == "UndefinedBindingResult")
                {
                    writer.Write("ifCond:args[2] undefined");
                    return;
                }
                if (args[0].GetType().Name == "String")
                {
                    var val1 = args[0].ToString();
                    var val2 = args[2].ToString();

                    if (args[1].ToString() == ">")
                    {
                        if (val1.Length > val2.Length)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                    else if (args[1].ToString() == "=" || args[1].ToString() == "==")
                    {
                        if (val1 == val2)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                    else if (args[1].ToString() == "<")
                    {
                        if (val1.Length < val2.Length)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                    else if (args[1].ToString() == "!=" || args[1].ToString() == "<>")
                    {
                        if (val1 != val2)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                }
                else
                {
                    var val1 = Single.Parse(args[0].ToString());
                    var val2 = Single.Parse(args[2].ToString());

                    if (args[1].ToString() == ">")
                    {
                        if (val1 > val2)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                    else if (args[1].ToString() == "=" || args[1].ToString() == "==")
                    {
                        if (Math.Abs(val1 - val2) < 1)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                    else if (args[1].ToString() == "<")
                    {
                        if (val1 < val2)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                    else if (args[1].ToString() == "!=" || args[1].ToString() == "<>")
                    {
                        if (Math.Abs(val1 - val2) > 0)
                        {
                            options.Template(writer, (Object)context);
                        }
                        else
                        {
                            options.Inverse(writer, (Object)context);
                        }
                    }
                }
            });

            _handlebars.RegisterHelper("partial-helper", (output, options, context, args) =>
            {
                options.Template(output, context);
            });

            _handlebars.RegisterHelper("ifContext", (output, options, context, args) =>
            {
                if (String.Equals(args[0].GetType().Name, $"{args[1]}TemplateModel", StringComparison.InvariantCultureIgnoreCase))
                {
                    options.Template(output, context);
                    return;
                }

                options.Inverse(output, context);
            });
        }

        private void RegisterTemplates()
        {
            RegisterTemplates("shared");
            RegisterTemplates("layouts");
        }

        private void RegisterTemplates(String subDir)
        {
            var dirPath = Theme.Directory.CombineDirPath(subDir);
            var templates = dirPath.GetFiles("*.hbs", SearchOption.AllDirectories);
            foreach (var partial in templates)
            {
                //var dir = partial.DirectoryName.Replace(Theme.Directory.FullName, "")
                //    .Replace("\\shared", "")
                //    .Replace("\\layouts", "")
                //    .Replace("\\_layouts", "layouts");

                var dir = partial.DirectoryName?.Replace(dirPath.FullName, "") ?? "";

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
            var outputDir = SiteConfig.Paths.OutputPath.CombineDirPath(model.Permalink.Replace("//", "/").Substring(1));
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }

            var file = outputDir.CombineFilePath(".html", "index");

            if (!_handlebars.Configuration.RegisteredTemplates.ContainsKey(template))
            {
                throw new Exception($"Cannot find templte for \"{template}\".");
            }

            var handlebarsTemplate = _handlebars.Configuration.RegisteredTemplates[template];

            using (var write = file.CreateText())
            {
                handlebarsTemplate(write, model);
            }
        }
    }
}
