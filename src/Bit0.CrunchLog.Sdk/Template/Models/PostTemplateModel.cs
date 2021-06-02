using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Sdk.Helpers;
using Bit0.CrunchLog.Template.Models.MetaData;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Template.Models
{
    public class PostTemplateModel : ITemplateModel
    {
        public PostTemplateModel(IContent content, CrunchConfig siteConfig, Boolean inList = false)
        {
            Id = content.Id;
            Title = content.Title;
            Description = content.Intro;
            DisclaimMessage = content.DisclaimMessage;
            Author = content.Author.Alias;
            Published = content.DatePublished;
            Updated = content.DateUpdated;
            Updates = content.Updates.Select(u => new PostUpdateModel { UpdatedOn = u.Key, Message = u.Value });
            Permalink = content.Permalink;
            DefaultCategory = content.Categories.FirstOrDefault().Key;
            IsDraft = !content.IsPublished;
            Image = content.Image.Url;
            ImagePlaceholder = content.Image.Placeholder;

            if(!inList)
            {
                Layout = content.Layout.GetValue();
                Content = content.Html;
                Categories = content.Categories.Select(c => c.Key);
                Keywords = content.Tags.Select(t => t.Value);
                Meta = content.GetMetaData(siteConfig);
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
        [JsonProperty("disclaimMessage")]
        public String DisclaimMessage { get; }
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
        [JsonProperty("updates")]
        public IEnumerable<PostUpdateModel> Updates { get; }
        [JsonProperty("image")]
        public String Image { get; set; }
        [JsonProperty("imagePlaceholder")]
        public String ImagePlaceholder { get; set; }
        [JsonProperty("defaultCategory")]
        public String DefaultCategory { get; }
        [JsonIgnore]
        public Boolean IsDraft { get; }
        [JsonProperty("meta")]
        public PostMetaData Meta { get; set; }

        public override String ToString() => Permalink;
    }
}