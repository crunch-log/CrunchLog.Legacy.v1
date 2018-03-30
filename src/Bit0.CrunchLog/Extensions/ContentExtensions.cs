using System;
using System.IO;
using System.Text;

namespace Bit0.CrunchLog.Extensions
{
    public static class ContentExtensions
    {
        public static void WriteFile(this Content content, DirectoryInfo outputDir)
        {
            var fullSlug = content.GetFullSlug();
            var outDir = new DirectoryInfo(outputDir.CombinePath(fullSlug.Substring(1)).NormalizePath());

            if (!outDir.Exists)
            {
                outDir.Create();
            }

            var file = new FileInfo(outDir.CombinePath("index.html"));
            using (var writer = file.CreateText())
            {
                var sb = new StringBuilder();
                sb.AppendLine($"<h1>{content.Title}</h1>");
                sb.AppendLine($"<p>{content.Intro}</p>");
                sb.AppendLine(content.Text);

                writer.Write(sb.ToString());
            }
        }

        private static String GetFullSlug(this Content content)
        {
            return content.PermaLink
                    .Replace(":year", content.Date.ToString("yyyy"))
                    .Replace(":month", content.Date.ToString("MM"))
                    .Replace(":day", content.Date.ToString("dd"))
                    .Replace(":slug", content.Slug)
                ;
        }
    }
}
