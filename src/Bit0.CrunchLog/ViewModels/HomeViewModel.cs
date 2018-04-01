using System.Collections.Generic;

namespace Bit0.CrunchLog.ViewModels
{
    public class HomeViewModel : PostViewModel
    {
        public IEnumerable<TagViewModel> Tags {get; set; }
        public IEnumerable<CategoryViewModel> Categories {get; set; }
        public IEnumerable<ArchiveViewModel> Archives {get; set; }
        public IEnumerable<PostViewModel> Posts {get; set; }
    }
}
