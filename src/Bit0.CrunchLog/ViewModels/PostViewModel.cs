using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        { }

        public PostViewModel(Content post, IEnumerable<String> keywords)
        {
            Title = post.Title;
            Content = post.Text;
            Description = post.Intro;
            Author = post.Author;
            Date = post.Date;
            Keywords = keywords.Concat(post.Tags);
            PermaLink = post.PermaLink;
        }


        public String Title { get; set; }

        public IEnumerable<String> Keywords { get; set; }

        public String Description { get; set; }

        public String Content { get; set; }

        public String PermaLink { get; set; }

        public Author Author { get; set; }

        public DateTime Date { get; set; }

        public override String ToString()
        {
            return PermaLink;
        }
    }
}
