using Bit0.CrunchLog.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Template.Models.MetaData
{
    public class PostMetaData
    {
        [JsonProperty("title")] public String Title { get; set; }
        [JsonProperty("description")] public String Description { get; set; }
        [JsonProperty("keywords")] public String Keywords { get; set; }
        [JsonProperty("publishedDate")] public DateTime PublishedDate { get; set; }
        [JsonProperty("updatedDate")] public DateTime UpdatedDate { get; set; }
        [JsonProperty("category")] public String Category { get; set; }
        [JsonProperty("robots")] public String Robots { get; set; } = "index, follow";
        [JsonProperty("shortUrl")] public String ShortUrl { get; set; }
        [JsonProperty("archive")] public ArchiveMetaData Archive { get; set; }
        [JsonProperty("type")] public String Type { get; set; }
        [JsonProperty("canonicalUrl")] public String CanonicalUrl { get; set; }
        [JsonProperty("language")] public String Language { get; set; }
        [JsonProperty("image")] public SiteImage Image { get; set; }
        [JsonProperty("redirect")] public RedirectMetaData Redirect { get; set; }
        [JsonProperty("social")] public IDictionary<String, String> Social { get; set; }
    }

    public class ListMetaData : PostMetaData
    {

    }
}
