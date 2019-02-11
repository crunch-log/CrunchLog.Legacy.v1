using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bit0.CrunchLog.Template.Models
{
    public class PostListTemplateModel : ITemplateModel
    {
        [JsonProperty("url")]
        public String Permalink { get; set; }
        [JsonIgnore]
        public String Layout { get; private set; }
        [JsonProperty("title")]
        public String Title { get; private set; }
        [JsonProperty("posts")]
        public IEnumerable<ITemplateModel> Posts { get; set; }
        [JsonProperty("pagination")]
        public IDictionary<Int32, PaginationTemplateModel> Pagination { get; set; }
        [JsonProperty("totalPages")]
        public Int32 TotalPages { get; set; }
        [JsonProperty("prevPageUrl")]
        public String PreviousPageUrl { get; set; }
        [JsonProperty("nextPageUrl")]
        public String NextPageUrl { get; set; }
        [JsonProperty("pageCount")]
        public Int32 PostsCount => Posts.Count();
        [JsonProperty("isHomeLayout")]
        public Boolean IsHomeLayout { get; }

        public PostListTemplateModel(
            ContentListItem contentListItem, CrunchSite config,
            Int32 page, Int32 totalPages)
        {
            TotalPages = totalPages;
            Pagination = Enumerable.Range(1, totalPages).ToDictionary(k => k, i => new PaginationTemplateModel
            {
                Page = i,
                Url = contentListItem.GetPagePermaLink(i),
                IsCurrentPage = page == i
            });

            PreviousPageUrl = page > 1 ? contentListItem.GetPagePermaLink(page - 1) : String.Empty;
            NextPageUrl = page < totalPages ? contentListItem.GetPagePermaLink(page + 1) : String.Empty;

            Layout = contentListItem.Layout.GetValue();
            IsHomeLayout = contentListItem.Layout == Layouts.Home;
            Permalink = Pagination[page].Url;
            Title = contentListItem.Title;

            var pageSize = config.Pagination.PageSize;

            Posts = contentListItem.Children
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Cast<Content>()
                .Select(c => c.GetModel(inList: true));
        }

        public override String ToString()
        {
            return Permalink;
        }
    }
}
