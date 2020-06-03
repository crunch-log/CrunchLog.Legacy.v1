using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Models.MetaData;
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
        public PaginationListTemplateModel Pagination { get; set; }

        [JsonProperty("pageCount")]
        public Int32 PostsCount => Posts.Count();
        [JsonProperty("isHomeLayout")]
        public Boolean IsHomeLayout { get; }

        [JsonProperty("meta")]
        public ListMetaData Meta { get; set; }

        public PostListTemplateModel(
            IContentListItem contentListItem, CrunchConfig config,
            Int32 page, Int32 totalPages)
        {
            Pagination = Enumerable.Range(1, totalPages).ToDictionary(k => k, i => new PaginationPageTemplateModel
            {
                Page = i,
                Url = contentListItem.GetPagePermaLink(i),
                IsCurrentPage = page == i
            }).GetModel(totalPages);

            Layout = contentListItem.Layout.GetValue();
            IsHomeLayout = contentListItem.Layout == Layouts.Home;
            Permalink = Pagination.AllPages[page].Url;
            Title = contentListItem.Title;

            var pageSize = config.Pagination.PageSize;

            Posts = contentListItem.Children
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Cast<IContent>()
                .Select(c => c.GetModel(config, inList: true));

            Meta = contentListItem.GetMetaData(config, Posts);
        }

        public override String ToString()
        {
            return Permalink;
        }
    }
}
