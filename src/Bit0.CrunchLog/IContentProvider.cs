using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public interface IContentProvider
    {
        IDictionary<String, Content> AllContent { get; }
        IEnumerable<Content> PublishedContent { get; }
        IEnumerable<Content> DraftContent { get; }
        IEnumerable<Content> Posts { get; }
        IEnumerable<Content> Pages { get; }
        ContentListItem Home { get; }
        IEnumerable<ContentListItem> Tags { get; }
        IEnumerable<ContentListItem> Categories { get; }
        IEnumerable<ContentListItem> PostArchives { get; }
        IDictionary<String, IContent> Links { get; }
        IEnumerable<ContentListItem> Authors { get; }
    }
}