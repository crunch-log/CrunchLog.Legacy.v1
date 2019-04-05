using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ScribanTemplate = Scriban.Template;

namespace Bit0.CrunchLog.Plugins.ScribanEngine
{
    internal class ScribanTemplateEngine : ITemplateEngine
    {
        private readonly CrunchSite _siteConfig;
        private readonly ILogger<ScribanEnginePlugin> _logger;
        private readonly IContentProvider _contentProvider;

        public ScribanTemplateEngine(CrunchSite siteConfig, ILogger<ScribanEnginePlugin> logger, IContentProvider contentProvider)
        {
            _siteConfig = siteConfig;
            _logger = logger;
            _contentProvider = contentProvider;
        }

        public void PreProcess() { }
        public void PostProcess(CrunchSite siteConfig, Theme theme) { }

        public void Render(ITemplateModel model)
        {
            var outputDir = _siteConfig.Paths.OutputPath;

            if (model is RedirectsTemplateModel)
            {
                //Render(model, "Redirect", outputDir.CombineFilePath(".json", "redirects"));
                return;
            }
            if (model is PostTemplateModel m)
            {
                outputDir = !m.IsDraft ? outputDir.CombineDirPath(m.Permalink.Substring(1)) : outputDir.CombineDirPath("draft", m.Id);
                Render(model, "Post", outputDir.CombineFilePath(".html", "index"));
                return;
            }
            if (model is PostListTemplateModel)
            {
                outputDir = outputDir.CombineDirPath(model.Permalink.Replace("//", "/").Substring(1));
                Render(model, "List", outputDir.CombineFilePath(".html", "index"));
                return;
            }

        }

        private void Render<TModel>(TModel model, String viewName, FileInfo outputFile) where TModel : ITemplateModel
        {
            if (!outputFile.Directory.Exists)
            {
                _logger.LogDebug($"Create directory: {outputFile.Directory.FullName}");
                outputFile.Directory.Create();
            }

            _logger.LogDebug($"Render template from view: {viewName}");

            using (var sw = outputFile.CreateText())
            {
                var content = RenderView(viewName, model);
                sw.WriteLine(content);
            }
        }

        private String RenderView<TModel>(String viewName, TModel model) where TModel : ITemplateModel
        {
            var context = new TemplateContext
            {
                TemplateLoader = new CrunchTemplateLoader(_siteConfig.Theme.Directory),
                MemberRenamer = member => member.Name,
                MemberFilter = member => member is PropertyInfo
            };

            var contextObj = new ScriptObject();
            contextObj["site"] = _siteConfig.GetModel(_contentProvider);
            contextObj["model"] = model;
            contextObj["title"] = model.Title;
            contextObj.SetValue("dump", new DumpFunction(), true);

            context.PushGlobal(contextObj);

            var template = ScribanTemplate.Parse("{{ include '" + viewName + "' }}");
            var content = FormatHtml(template.Render(context));
            return content;
        }

        private static String FormatHtml(String content)
        {
            var parser = new HtmlParser();
            var sw = new StringWriter();
            var document = parser.ParseDocument(content);
            document.ToHtml(sw, new PrettyMarkupFormatter());
            content = sw.ToString();
            return content;
        }
    }

    public class DumpFunction : ScriptObject, IScriptCustomFunction
    {
        public Object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
        {
            return $"<pre>{JsonConvert.SerializeObject(arguments, Formatting.Indented)}</pre>";
        }

        public ValueTask<Object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
        {
            return new ValueTask<Object>(Invoke(context, callerContext, arguments, blockStatement));
        }
    }
}