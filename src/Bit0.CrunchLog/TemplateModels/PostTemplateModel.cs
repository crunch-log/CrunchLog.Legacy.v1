using System;
using System.Collections.Generic;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;

namespace Bit0.CrunchLog.TemplateModels
{
    public class PostTemplateModel : ITemplateModel
    {
        public PostTemplateModel(Content content, CrunchSite siteConfig)
        {
            Title = content.Title;
            Content = content.Html;
            Description = content.Intro;
            Author = content.Author;
            Date = content.Date;
            Keywords = content.Tags;
            Permalink = content.Permalink;
            Layout = content.Layout.GetValue();
            Categories = content.Categories;
            BannerImage = content.BannerImage.ToRelative(siteConfig.Paths.ContentPath);

            Site = siteConfig.GetModel();
        }

        public String Layout { get; }
        public IDictionary<String, String> Categories { get; }
        public String Title { get; }
        public IDictionary<String, String> Keywords { get; }
        public String Description { get; }
        public String Content { get; }
        public String Permalink { get; set; }
        public Author Author { get; }
        public DateTime Date { get; }
        public SiteTemplateModel Site { get; set; }
        public String BannerImage { get; set; }
        public Boolean IsContentLayout => true;

        public override String ToString()
        {
            return Permalink;
        }
    }
}