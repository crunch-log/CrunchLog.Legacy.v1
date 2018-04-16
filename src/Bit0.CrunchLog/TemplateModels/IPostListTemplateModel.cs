using System;
using System.Collections.Generic;
using System.Linq;

namespace Bit0.CrunchLog.TemplateModels
{
    public interface IPostListTemplateModel : ITemplateModel
    {
        String Name { get; set; }
        IEnumerable<PostTemplateModel> Posts {get; set; }
    }

    public abstract class PostListBaseTemplateModel : IPostListTemplateModel
    {
        public String Name { get; set; }
        public IEnumerable<PostTemplateModel> Posts {get; set; }

        public String Permalink { get; set; }
        public abstract String Layout { get; }

        public override String ToString()
        {
            return $"Link: {Permalink}, Count: {Posts.Count()}";
        }
    }
}