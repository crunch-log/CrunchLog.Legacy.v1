using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.Template.Models
{
    public class PaginationListTemplateModel
    {
        [JsonProperty("allPages")]
        public IDictionary<Int32, PaginationPageTemplateModel> AllPages { get; set; }
        [JsonProperty("pageSpan")]
        public IEnumerable<PaginationPageTemplateModel> PageSpan { get; set; }
        [JsonProperty("firstPage")]
        public PaginationPageTemplateModel FirstPage { get; set; }
        [JsonProperty("lastPage")]
        public PaginationPageTemplateModel LastPage { get; set; }
        [JsonProperty("nextPage")]
        public PaginationPageTemplateModel NextPage { get; set; }
        [JsonProperty("previousPage")]
        public PaginationPageTemplateModel PreviousPage { get; set; }
        [JsonProperty("totalPages")]
        public Int32 TotalPages { get; set; }
        [JsonProperty("currentPage")]
        public Int32 CurrentPage { get; set; }
    }
}
