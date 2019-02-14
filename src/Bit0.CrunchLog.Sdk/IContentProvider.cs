using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public interface IContentProvider
    {
        IDictionary<String, IContent> AllContent { get; }
        IEnumerable<IContent> PublishedContent { get; }
        IEnumerable<IContent> DraftContent { get; }
        IEnumerable<IContent> Posts { get; }
        IEnumerable<IContent> Pages { get; }
        IContentListItem Home { get; }
        IEnumerable<IContentListItem> Tags { get; }
        IEnumerable<IContentListItem> Categories { get; }
        IEnumerable<IContentListItem> PostArchives { get; }
        IDictionary<String, IContentBase> Links { get; }
        IEnumerable<IContentListItem> Authors { get; }
    }
}