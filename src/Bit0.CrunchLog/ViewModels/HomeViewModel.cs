using System.Collections.Generic;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(CrunchConfig config)
        {
            Config = config;
        }

        public CrunchConfig Config { get; }

        public IEnumerable<TagViewModel> Tags {get; set; }
        public IEnumerable<CategoryViewModel> Categories {get; set; }
        public IEnumerable<ArchiveViewModel> Archives {get; set; }
        public IEnumerable<PostViewModel> Posts {get; set; }
    }
}
