using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.JsonConverters;
using Markdig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Bit0.CrunchLog
{
    public class Content : IContent
    {
        private const String _regex = @"(^[0-9]{1,4})-(.*)\.md$";
        private readonly CrunchSite _siteConfig;

        public Content()
        { }

        public Content(FileInfo contentFile, CrunchSite siteConfig)
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

        [JsonProperty("dateUpdated")]
        public DateTime DateUpdated { get; set; } = DateTime.MinValue;

        [JsonProperty("tags")]
        [JsonConverter(typeof(ListConverter), Layouts.Tag)]
        public IDictionary<String, CategoryInfo> Tags { get; set; }

        [JsonProperty("categories")]
        [JsonConverter(typeof(ListConverter), Layouts.Category)]
        public IDictionary<String, CategoryInfo> Categories { get; set; }

        [JsonIgnore]
        public CategoryInfo DefaultCategory { get; set; }

        [JsonProperty("published")]
        public Boolean Published { get; set; }

        [JsonProperty("intro")]
        public String Intro { get; set; }

        [JsonProperty("permaLink")]
        public String Permalink { get; set; }

        [JsonProperty("author")]
        [JsonConverter(typeof(AuthorConverter))]
        public Author Author { get; set; }

        [JsonProperty("bannerImage")]
        public String BannerImage { get; set; }

        [JsonProperty("thumb")]
        public String Thumbnail { get; set; }

        [JsonIgnore]
        public FileInfo ContentFile { get; }

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
                return Markdown.ToHtml(ContentFile.GetText(), pipeline);
            }
        }

        public override String ToString() => $"{Id:00000} {Permalink}";

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var match = Regex.Match(ContentFile.Name, _regex);

            if (String.IsNullOrWhiteSpace(Id) && match.Success)
            {
                Id = match.Groups[1].Value;
            }

            if (String.IsNullOrWhiteSpace(Slug) && match.Success)
            {
                Slug = match.Groups[2].Value;
            }

            // fix permalink
            Permalink = Permalink
                .Replace(":year", DatePublished.ToString("yyyy"))
                .Replace(":month", DatePublished.ToString("MM"))
                .Replace(":day", DatePublished.ToString("dd"))
                .Replace(":slug", Slug);

            if (Author == null)
            {
                Author = _siteConfig.Authors.FirstOrDefault().Value;
            }

            DefaultCategory = Categories.FirstOrDefault().Value;
            if (String.IsNullOrWhiteSpace(BannerImage))
            {
                BannerImage = !String.IsNullOrWhiteSpace(DefaultCategory.Image)
                    ? DefaultCategory.Image
                    : _siteConfig.DefaultBanner;
            }

            if (String.IsNullOrWhiteSpace(Thumbnail))
            {
                Thumbnail = !String.IsNullOrWhiteSpace(DefaultCategory.Thumbnail)
                    ? DefaultCategory.Thumbnail
                    : _siteConfig.DefaultThumbnail;
            }

            if (DateUpdated == DateTime.MinValue)
            {
                DateUpdated = DatePublished;
            }
        }
    }
}
