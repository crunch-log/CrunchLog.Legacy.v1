using Bit0.CrunchLog.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Template.Models.MetaData
{
    public class SiteMetaData
    {
        [JsonProperty("title")] public String Title { get; set; }
        [JsonProperty("subTitle")] public String SubTitle { get; set; }
        [JsonProperty("designer")] public String Designer { get; set; }
        [JsonProperty("copyright")] public String Copyright { get; set; }
        [JsonProperty("description")] public String Description { get; set; }
        [JsonProperty("manifest")] public String Manifest { get; set; }
        [JsonProperty("themeColor")] public String ThemeColor { get; set; }
        [JsonProperty("canonicalUrl")] public String CanonicalUrl { get; set; }
        [JsonProperty("social")] public IDictionary<String, String> Social { get; set; }
        [JsonProperty("icon")] public IconMetaData Icon { get; set; }
        [JsonProperty("generator")] public String Generator => $"CrunchLog {typeof(CrunchSite).Assembly.GetName().Version.ToString()}";
        [JsonProperty("baseUrl")]  public String BaseUrl { get; set; }
        [JsonProperty("language")]  public String Language { get; set; }
    }
}
