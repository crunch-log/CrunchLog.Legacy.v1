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
            Title = content.Title;
            Description = content.Intro;
            Author = content.Author;
            Date = content.Date;
            Keywords = content.Tags.Select(t => new CategoryInfo
            {
                Title = t.Key,
                Permalink = t.Value,
                Color = ""
            });
            Permalink = content.Permalink;
            Categories = content.Categories.Select(c => new CategoryInfo
            {
                Title = c.Key,
                Permalink = c.Value,
                Color = siteConfig.Categories[siteConfig.Categories.ContainsKey(c.Key) ? c.Key : "Default"]
            });
            BannerImage = content.BannerImage.ToRelative(siteConfig.Paths.ContentPath);

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
        public String Title { get; }
        public IEnumerable<CategoryInfo> Keywords { get; }
        public String Description { get; }
        public String Content { get; }
        public String Permalink { get; set; }
        public Author Author { get; }
        public DateTime Date { get; }
        public SiteTemplateModel Site { get; set; }
        public String BannerImage { get; set; }
        public Boolean IsContentLayout => true;
        public CategoryInfo DefaultCategory { get; }

        public override String ToString()
        {
            return Permalink;
        }
    }
}