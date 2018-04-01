using System;
using System.Collections.Generic;
using System.Linq;

namespace Bit0.CrunchLog.ViewModels
{
    public interface IPostListViewModel
    {
        String Name { get; set; }
        IEnumerable<PostViewModel> Posts {get; set; }
    }

    public class PostListBaseViewModel : IPostListViewModel
    {
        public String Name { get; set; }
        public IEnumerable<PostViewModel> Posts {get; set; }

        public override String ToString()
        {
            return $"{Name} / Count: {Posts.Count()}";
        }
    }
}