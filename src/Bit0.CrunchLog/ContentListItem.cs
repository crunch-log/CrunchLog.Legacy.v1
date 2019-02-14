using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public class ContentListItem : IContentListItem
    {
        public Layouts Layout { get; set; }
        public String Permalink { get; set; }
        public String Title { get; set; }
        public IEnumerable<IContent> Children { get; set; }
    }
}