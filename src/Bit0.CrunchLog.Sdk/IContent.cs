using System;
using System.Collections.Generic;
using System.IO;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog
{
    public interface IContent : IContentBase
    {
        String Id { get; set; }
        String Slug { get; set; }
        String Intro { get; set; }
        DateTime DatePublished { get; set; }
        DateTime DateUpdated { get; set; }
        IDictionary<DateTime, String> Updates { get; set; }
        IDictionary<String, CategoryInfo> Tags { get; set; }
        IDictionary<String, CategoryInfo> Categories { get; set; }
        CategoryInfo DefaultCategory { get; set; }
        Boolean IsPublished { get; set; }
        Author Author { get; set; }
        String DisclaimMessage { get; set; }
        SiteImage Image { get; set; }
        FileInfo ContentFile { get; }
        String Html { get; }
        String ShortUrl { get; }
        IEnumerable<String> Redirects { get; set; }
    }
}