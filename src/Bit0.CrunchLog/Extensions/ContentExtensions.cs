using System.IO;
using System.Text;
using Bit0.CrunchLog.ViewModels;

namespace Bit0.CrunchLog.Extensions
{
    public static class ContentExtensions
    {
        public static void WriteFile(this Content content, DirectoryInfo outputDir)
        {
            var permaLink = content.PermaLink;
            var outDir = new DirectoryInfo(outputDir.CombinePath(permaLink.Substring(1)).NormalizePath());

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

        public static void WriteFile(this IPostListViewModel list, DirectoryInfo outputDir)
        {

        }
    }
}
