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

    public class PostListBaseTemplateModel : IPostListTemplateModel
    {
        public String Name { get; set; }
        public IEnumerable<PostTemplateModel> Posts {get; set; }

        public String Permalink { get; set; }

        public override String ToString()
        {
            return $"Link: {Permalink}, Count: {Posts.Count()}";
        }
    }
}