using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.TemplateModels
{
    public class PostTemplateModel : ITemplateModel
    {
        public PostTemplateModel(Content content, CrunchConfig config)
        {
            Title = content.Title;
            Content = content.Html;
            Description = content.Intro;
            Author = content.Author;
            Date = content.Date;
            Keywords = config.Tags.Concat(content.Tags);
            Permalink = content.Permalink;
            Layout = content.Layout;
            Categories = content.Categories;

            Config = config;
        }

        public String Layout { get; }
        public IEnumerable<String> Categories { get; }
        public String Title { get; }
        public IEnumerable<String> Keywords { get; }
        public String Description { get; }
        public String Content { get; }
        public String Permalink { get; set; }
        public Author Author { get; }
        public DateTime Date { get; }
        public CrunchConfig Config { get; set; }

        public override String ToString()
        {
            return Permalink;
        }
    }
}