using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel(CrunchConfig config, Content post)
        {
            Title = post.Title;
            Content = post.Text;
            Description = post.Intro;
            Author = post.Author;
            Date = post.Date;
            Keywords = config.Tags.Concat(post.Tags);
            PermaLink = post.PermaLink;

            Config = config;
        }


        public String Title { get; }

        public IEnumerable<String> Keywords { get; }

        public String Description { get; }

        public String Content { get; }

        public String PermaLink { get; }

        public Author Author { get; }

        public DateTime Date { get; }

        public CrunchConfig Config { get; }

        public override String ToString()
        {
            return PermaLink;
        }
    }
}
