using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public class ContentListItem : IContentListItem
    {
        public Layouts Layout { get; set; }
        public String Permalink { get; set; }
        public String Title { get; set; }
        public String Name { get; set; }
        public IEnumerable<IContent> Children { get; set; }
    }

    public class AuthorListItem : ContentListItem
    {
        public String Alias { get; set; }
        public String Email { get; set; }
        public String Homepage { get; set; }
        public String Description { get; set; }
        public IDictionary<String, String> Social { get; set; }
    }
}