using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Extensions;
using Markdig;
using Newtonsoft.Json;

namespace Bit0.CrunchLog
{
    public class Content
    {
        private String _mdFile;
        private String _slug;

        [JsonProperty("content")]
        public String MarkdownFile
        {
            get
            {
                // if content file is not defined
                if (String.IsNullOrWhiteSpace(_mdFile))
                {
                    var dir = MetaFile?.Directory;

                    // find post.md
                    if (dir?.GetFiles("post.md", SearchOption.TopDirectoryOnly)
                            .SingleOrDefault() != null)
                    {
                        return "post.md";
                    }

                    // find content.md
                    if (dir?.GetFiles("content.md", SearchOption.TopDirectoryOnly)
                            .SingleOrDefault() != null)
                    {
                        return "content.md";
                    }

                    // find <dirName>.md
                    if (dir?.GetFiles($"{dir?.Name}.md", SearchOption.TopDirectoryOnly)
                            .SingleOrDefault() != null)
                    {
                        return $"{dir?.Name}.md";
                    }

                    // fnd <jsonFileName>.md
                    if (dir?.GetFiles($"{MetaFile.Name.Replace(".json", "")}.md", SearchOption.TopDirectoryOnly)
                            .SingleOrDefault() != null)
                    {
                        return $"{MetaFile.Name.Replace(".json", "")}.md";
                    }

                    throw new FileNotFoundException($"Could not find fil for {MetaFile?.Name}");
                }

                return _mdFile;
            }
            set => _mdFile = value;
        }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("layout")]
        public String Layout { get; set; } = Layouts.Post;

        [JsonProperty("slug")]
        public String Slug
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_slug))
                {
                    return MetaFile?.Directory?.Name;
                }

                return _slug;
            }

            set => _slug = value;
        }

        [JsonProperty("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [JsonProperty("tags")]
        public IEnumerable<String> Tags { get; set; }

        [JsonProperty("categories")]
        public IEnumerable<String> Categories { get; set; }

        [JsonProperty("published")]
        public Boolean Published { get; set; }

        [JsonProperty("intro")]
        public String Intro { get; set; }

        [JsonIgnore]
        public String PermaLink { get; set; }

        [JsonIgnore]
        public FileInfo MetaFile { get; set; }

        [JsonIgnore]
        public FileInfo ContentFile => new FileInfo(MetaFile?.Directory?.CombinePath(MarkdownFile));

        [JsonIgnore]
        public String Text
        {
            get
            {
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                return Markdown.ToHtml(ContentFile.GetText(), pipeline);
            }
        }

        [JsonIgnore]
        public Content Parent => null;

        [JsonIgnore]
        public IEnumerable<Content> Children => null;
    }

    public static class Layouts
    {
        public const String Post = "post";
        public const String Page = "page";
    }
}
