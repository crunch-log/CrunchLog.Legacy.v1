using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;

namespace Bit0.CrunchLog.Template.Models
{
    public class PostTemplateModel : ITemplateModel
    {
        public PostTemplateModel(Content content, CrunchSite siteConfig, Boolean inList = false)
        {
            Id = content.Id;
            Title = content.Title;
            Description = content.Intro;
            Author = content.Author;
            Published = content.DatePublished;
            Updated = content.DateUpdated;
            Keywords = content.Tags.Select(t => t.Value);
            Permalink = content.Permalink;
            Categories = content.Categories.Select(c => c.Value);
            BannerImage = content.BannerImage;

            DefaultCategory = Categories.FirstOrDefault();

            if (!inList)
            {
                Layout = content.Layout.GetValue();
                Content = content.Html;
                Site = siteConfig.GetModel();
            }
        }

        public String Layout { get; }
        public IEnumerable<CategoryInfo> Categories { get; }
        public String Id { get; set; }
        public String Title { get; }
        public IEnumerable<CategoryInfo> Keywords { get; }
        public String Description { get; }
        public String Content { get; }
        public String Permalink { get; set; }
        public Author Author { get; }
        public DateTime Published { get; }
        public DateTime Updated { get; }
        public SiteTemplateModel Site { get; set; }
        public String BannerImage { get; set; }
        public Boolean IsContentLayout => true;
        public CategoryInfo DefaultCategory { get; }

        public override String ToString() => Permalink;
    }
}