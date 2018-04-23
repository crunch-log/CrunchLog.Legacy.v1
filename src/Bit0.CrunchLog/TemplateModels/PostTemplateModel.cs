using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.TemplateModels
{
    public class PostTemplateModel : ITemplateModel
    {
        public PostTemplateModel(CrunchConfig config, Content post)
        {
            Title = post.Title;
            Content = post.Text;
            Description = post.Intro;
            Author = post.Author;
            Date = post.Date;
            Keywords = config.Tags.Concat(post.Tags);
            Permalink = post.Permalink;
            Layout = post.Layout;
            Categories = post.Categories;

            Config = config;
        }

        public String Layout { get; }
        public IEnumerable<string> Categories { get; }
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
