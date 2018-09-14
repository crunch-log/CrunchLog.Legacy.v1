using Bit0.CrunchLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Extensions;

namespace Bit0.CrunchLog.Template.Models
{
    public class PostListTemplateModel : ITemplateModel
    {
        public SiteTemplateModel Site { get; set; }
        public String Permalink { get; set; }
        public String Layout { get; private set; }
        public String Title { get; private set; }
        public IEnumerable<ITemplateModel> Posts { get; set; }
        public IDictionary<Int32, PaginationTemplateModel> Pagination { get; set; }
        public Int32 TotalPages { get; set; }
        public String PreviousPageUrl { get; set; }
        public String NextPageUrl { get; set; }
        public Int32 PostsCount => Posts.Count();
        public Boolean IsHomeLayout => Layout.Equals(Layouts.Home);

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
            Permalink = Pagination[page].Url;
            Title = contentListItem.Title;
            Site = config.GetModel();

            var pageSize = config.Pagination.PageSize;

            Posts = contentListItem.Children
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Cast<Content>()
                .Select(c => c.GetModel(config));
        }

        public override String ToString()
        {
            return Permalink;
        }
    }
}
