using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
using Bit0.CrunchLog.JsonConverters;
using Markdig;
using Newtonsoft.Json;

namespace Bit0.CrunchLog
{
    public class Content : IContent
    {
        private readonly CrunchConfig _siteConfig;

        public Content()
        { }

        public Content(FileInfo contentFile, CrunchConfig siteConfig)
        {
            _siteConfig = siteConfig;

            ContentFile = contentFile;
            Permalink = _siteConfig.Permalink;
        }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("layout")]
        public Layouts Layout { get; set; } = Layouts.Post;

        [JsonProperty("slug")]
        public String Slug { get; set; }

        [JsonProperty("datePublished")]
        public DateTime DatePublished { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public DateTime DateUpdated { get; set; } = DateTime.MinValue;

        [JsonProperty("updates")]
        public IDictionary<DateTime, String> Updates { get; set; } = new Dictionary<DateTime, String>();

        [JsonProperty("disclaimMessage")]
        public String DisclaimMessage { get; set; }

        [JsonProperty("tags")]
        [JsonConverter(typeof(ListConverter), Layouts.Tag)]
        public IDictionary<String, CategoryInfo> Tags { get; set; }

        [JsonProperty("categories")]
        [JsonConverter(typeof(ListConverter), Layouts.Category)]
        public IDictionary<String, CategoryInfo> Categories { get; set; }

        [JsonIgnore]
        public CategoryInfo DefaultCategory { get; set; }

        private Boolean _isPublished;

        [JsonProperty("published")]
        public Boolean IsPublished
        {
            get
            {
                return _isPublished && DatePublished < DateTime.UtcNow;
            }
            set
            {
                _isPublished = value;
            }
        }

        [JsonProperty("intro")]
        public String Intro { get; set; }

        [JsonProperty("permaLink")]
        public String Permalink { get; set; }

        [JsonProperty("shortUrl")]
        public String ShortUrl => String.Format(StaticKeys.PostPathFormat, Id.TrimStart('0'));

        [JsonProperty("authors")]
        [JsonConverter(typeof(AuthorConverter))]
        public IEnumerable<Author> Authors { get; set; } = new List<Author>();

        [JsonProperty("author")]
        [JsonConverter(typeof(AuthorConverter))]
        public IEnumerable<Author> Author { set { Authors = value; } }

        [JsonProperty("image")]
        public SiteImage Image { get; set; }

        [JsonIgnore]
        public FileInfo ContentFile { get; }

        [JsonProperty("redirects")]
        public IEnumerable<String> Redirects { get; set; } = new List<String>();

        [JsonIgnore]
        public String Html
        {
            get
            {
                var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseAutoIdentifiers()
                    .UseAutoLinks()
                    .UseTaskLists()
                    .UsePipeTables()
                    .UseGridTables()
                    .UseEmphasisExtras()
                    .UseGenericAttributes()
                    .UseFootnotes()
                    .UseAbbreviations()
                    .UseEmojiAndSmiley()
                    .UsePreciseSourceLocation()
                    .UseYamlFrontMatter()
                    .Build();
                return Markdown.ToHtml(ContentFile.ReadText(), pipeline);
            }
        }

        public override String ToString() => $"{Id:00000} {Permalink}";

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var filenameMatch = Regex.Match(ContentFile.Name, @"(^[0-9]{1,4})-(.*)\.md$");

            if(String.IsNullOrWhiteSpace(Id) && filenameMatch.Success)
            {
                Id = filenameMatch.Groups[1].Value;
            }

            if(String.IsNullOrWhiteSpace(Slug) && filenameMatch.Success)
            {
                Slug = filenameMatch.Groups[2].Value;
            }

            // fix permalink
            Permalink = Permalink
                .Replace(":year", DatePublished.ToString("yyyy"))
                .Replace(":month", DatePublished.ToString("MM"))
                .Replace(":day", DatePublished.ToString("dd"))
                .Replace(":slug", Slug);

            if(!Authors.Any())
            {
                Authors = new List<Author> { _siteConfig.Authors.FirstOrDefault().Value };
            }

            DefaultCategory = Categories.FirstOrDefault().Value;
            if(DefaultCategory.Image == null)
            {
                DefaultCategory.Image = _siteConfig.DefaultBannerImage;
            }

            if(Image == null)
            {
                Image = DefaultCategory.Image;
            }

            if(Updates.Keys.Any())
            {
                DateUpdated = Updates.Keys.Max();
            }

            if(DateUpdated < DatePublished)
            {
                DateUpdated = DatePublished;
            }

            // get disclaim message from config if needed
            if(!String.IsNullOrWhiteSpace(DisclaimMessage))
            {
                var disclaimMatch = Regex.Match(DisclaimMessage, @"^\[(?<disclaim>.*)\]$");
                if(disclaimMatch.Success)
                {
                    DisclaimMessage = _siteConfig.DisclaimMessages[disclaimMatch.Groups["disclaim"].Value];
                }
            }

            // redirect post id to permalink
            if(!Redirects.Contains(ShortUrl) && ShortUrl != String.Format(StaticKeys.PostPathFormat, ""))
            {
                Redirects = Redirects.Concat(new[] { ShortUrl });
            }

            // pad post id with Zero
            var paddedShortUrl = String.Format(StaticKeys.PostPathFormat, Id);
            if(!Redirects.Contains(paddedShortUrl) && paddedShortUrl != String.Format(StaticKeys.PostPathFormat, ""))
            {
                Redirects = Redirects.Concat(new[] { paddedShortUrl });
            }
        }
    }
}
