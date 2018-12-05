using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bit0.CrunchLog.Template.Models
{
    public class PostTemplateModel : ITemplateModel
    {
        public PostTemplateModel(Content content, Boolean inList = false)
        {
            Id = content.Id;
            Title = content.Title;
            Description = content.Intro;
            Author = content.Author.Alias;
            Published = content.DatePublished;
            Permalink = content.Permalink;
            DefaultCategory = content.Categories.FirstOrDefault().Key;

            if (content.Thumbnail != content.DefaultCategory.Thumbnail)
            {
                BannerImage = content.BannerImage;
                Thumbnail = content.Thumbnail;
                ThumbnailSmall = content.ThumbnailSmall;
            }

            if (!inList)
            {
                Layout = content.Layout.GetValue();
                Content = content.Html;
                Categories = content.Categories.Select(c => c.Key);
                Updated = content.DateUpdated;
                Keywords = content.Tags.Select(t => t.Value);
            }
        }

        [JsonIgnore]
        public String Layout { get; }
        [JsonProperty("categories")]
        public IEnumerable<String> Categories { get; }
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("title")]
        public String Title { get; }
        [JsonProperty("keywords")]
        public IEnumerable<CategoryInfo> Keywords { get; }
        [JsonProperty("description")]
        public String Description { get; }
        [JsonProperty("content")]
        public String Content { get; }
        [JsonProperty("url")]
        public String Permalink { get; set; }
        [JsonProperty("author")]
        public String Author { get; }
        [JsonProperty("published")]
        public DateTime Published { get; }
        [JsonProperty("updated")]
        public DateTime Updated { get; }
        [JsonProperty("image")]
        public String BannerImage { get; set; }
        [JsonProperty("thumb")]
        public String Thumbnail { get; set; }
        [JsonProperty("thumbSmall")]
        public String ThumbnailSmall { get; set; }
        [JsonProperty("defaultCategory")]
        public String DefaultCategory { get; }

        public override String ToString() => Permalink;
    }
}