using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bit0.CrunchLog.Template.ScribanEngine
{
    public class ScribanTemplateLoader : ITemplateLoader
    {
        private readonly DirectoryInfo _templateRoot;

        public ScribanTemplateLoader(DirectoryInfo templateRoot)
        {
            _templateRoot = templateRoot;
        }

        public String GetPath(TemplateContext context, SourceSpan callerSpan, String templateName)
        {
            var exts = new[]
{
                "scriban",
                "sbn",
                "scriban-html",
                "scriban-htm",
                "sbn-html",
                "sbn-htm",
                "sbnhtml",
                "sbnhtm",
                "scriban-txt",
                "sbn-txt",
                "sbntxt",
                "html",
                "htm",
                "txt"
            };

            FileInfo file = null;

            var ext = new FileInfo(templateName).Extension;
            file = String.IsNullOrWhiteSpace(ext)
                ? exts.SelectMany(x => _templateRoot.GetFiles($"{templateName}.{x}")).FirstOrDefault()
                : _templateRoot.GetFiles(templateName).FirstOrDefault();

            if (file != null)
            {
                return file.FullName;
            }

            throw new FileNotFoundException($"Could not find view. Looked in \"{_templateRoot.FullName}\" for:\r\n{exts.Select(x => $"\t{templateName}.{x}").Aggregate((a, b) => $"{a},\r\n{b}")}");
        }

        public String Load(TemplateContext context, SourceSpan callerSpan, String templatePath)
        {
            return File.ReadAllText(templatePath);
        }

        public ValueTask<String> LoadAsync(TemplateContext context, SourceSpan callerSpan, String templatePath)
        {
            return new ValueTask<String>(Load(context, callerSpan, templatePath));
        }
    }
}
