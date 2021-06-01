using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;
using ScribanTemplate = Scriban.Template;

namespace Bit0.CrunchLog.Template.ScribanEngine
{
    internal class ScribanTemplateEngine : ITemplateEngine
    {
        private readonly CrunchConfig _siteConfig;
        private readonly ILogger<ScribanTemplateEngine> _logger;
        private readonly IContentProvider _contentProvider;

        public ScribanTemplateEngine(CrunchConfig siteConfig, ILogger<ScribanTemplateEngine> logger, IContentProvider contentProvider)
        {
            _siteConfig = siteConfig;
            _logger = logger;
            _contentProvider = contentProvider;
        }

        public void PreProcess() { }
        public void PostProcess(CrunchConfig siteConfig, Theme theme) { }

        public void Render(ITemplateModel model)
        {
            var outputDir = _siteConfig.Paths.OutputPath;

            if(model is PostRedirectTemplateModel redirect)
            {
                outputDir = outputDir.CombineDirPath(redirect.RedirectUrl[1..]);
                Render(model, "Redirect", outputDir.CombineFilePath(".html", "index"));
            }
            else if(model is PostTemplateModel post)
            {
                outputDir = !post.IsDraft ? outputDir.CombineDirPath(post.Permalink[1..]) : outputDir.CombineDirPath("draft", post.Id);
                Render(model, "Post", outputDir.CombineFilePath(".html", "index"));
            }
            else if(model is NotFoundTemplateModel)
            {
                outputDir = outputDir.CombineDirPath("404");
                Render(model, "404", outputDir.CombineFilePath(".html", "index"));
            }
            else if(model is PostListTemplateModel)
            {
                outputDir = outputDir.CombineDirPath(model.Permalink.Replace("//", "/")[1..]);
                Render(model, "List", outputDir.CombineFilePath(".html", "index"));
            }
        }

        private void Render<TModel>(TModel model, String viewName, FileInfo outputFile) where TModel : ITemplateModel
        {
            if(!outputFile.Directory.Exists)
            {
                _logger.LogTrace($"Create directory: {outputFile.Directory.FullName}");
                outputFile.Directory.Create();
            }

            _logger.LogTrace($"Render template from view: {viewName} ({model.Permalink})");

            using(var sw = outputFile.CreateText())
            {
                var content = RenderView(viewName, model);
                sw.WriteLine(content);
            }
        }

        private String RenderView<TModel>(String viewName, TModel model) where TModel : ITemplateModel
        {
            var context = new TemplateContext
            {
                TemplateLoader = new ScribanTemplateLoader(_siteConfig.Theme.TemplateRoot),
                MemberRenamer = member => member.Name,
                MemberFilter = member => member is PropertyInfo
            };

            var contextObj = new ScriptObject
            {
                ["site"] = _siteConfig.GetModel(_contentProvider),
                ["model"] = model,
                ["title"] = model.Title
            };
            contextObj.SetValue("dump", new DumpFunction(), true);

            context.PushGlobal(contextObj);

            var template = ScribanTemplate.Parse("{{ include '" + viewName + "' }}");
            var content = template.Render(context);
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