using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public interface IContentProvider
    {
        IEnumerable<Content> AllContent { get; }
        IEnumerable<Content> PublishedContent { get; }
        IEnumerable<Content> Posts { get; }
        IEnumerable<Content> Pages { get; }
        ContentListItem Home { get; }
        IEnumerable<ContentListItem> PostTags { get; }
        IEnumerable<ContentListItem> PostCategories { get; }
        IEnumerable<ContentListItem> PostArchives { get; }
        IDictionary<String, IContent> Links { get; }
        IEnumerable<ContentListItem> Authors { get; }
    }
}