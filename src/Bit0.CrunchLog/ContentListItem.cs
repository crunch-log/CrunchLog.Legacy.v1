using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public class ContentListItem : IContent
    {
        public Layouts Layout { get; set; }
        public String Permalink { get; set; }
        public String Title { get; set; }
        public IEnumerable<IContent> Children { get; set; }
    }
}