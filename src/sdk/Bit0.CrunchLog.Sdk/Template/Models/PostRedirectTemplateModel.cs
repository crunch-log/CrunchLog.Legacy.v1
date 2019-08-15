using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models.MetaData;
using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models
{
    public class PostRedirectTemplateModel : PostTemplateModel, ITemplateModel
    {
        [JsonProperty("redirectUrl")]
        public String RedirectUrl { get; set; }

        public PostRedirectTemplateModel(IContent content, CrunchSite siteConfig, String redirectUrl)
            : base(content, siteConfig, inList: false)
        {
            RedirectUrl = redirectUrl;
            Meta.Redirect = new RedirectMetaData
            {
                Time = 0,
                Url = content.Permalink
            };
        }
    }
}