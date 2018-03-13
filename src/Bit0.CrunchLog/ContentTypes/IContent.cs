using System;
using System.Collections.Generic;
using System.IO;

namespace Bit0.CrunchLog.ContentTypes
{
    public interface IContent
    {
        String PermaLink { get; set; }

        FileInfo MetaFile { get; set; }
        FileInfo ContentFile { get; }

        String MarkdownFile { get; set; }
        String Title { get; set; }
        String Slug { get; set; }
        DateTime Date { get; set; }
        IEnumerable<String> Tags { get; set; }
        IEnumerable<String> Categories { get; set; }
        Boolean Published { get; set; }
        String Intro { get; set; }

        String Content { get; }

        IContent Parent { get; }
    }
}
