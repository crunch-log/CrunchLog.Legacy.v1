using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Template.Models
{
    public class NotFoundTemplateModel : ITemplateModel
    {
        [JsonProperty("url")]
        public String Permalink { get => "/404/"; set => _ = value; }
        [JsonIgnore]
        public String Layout => Layouts.NotFound.GetValue();
        [JsonProperty("title")]
        public String Title => "Not Found";
    }
}