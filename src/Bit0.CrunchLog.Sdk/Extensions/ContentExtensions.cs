using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bit0.CrunchLog.Extensions
{
    public static class ContentExtensions
    {
        public static PostTemplateModel GetModel(this IContent content, Boolean inList = false)
        {
            return new PostTemplateModel(content, inList);
        }

        public static RedirectsTemplateModel GetRedirectModel(this IEnumerable<IContent> contents)
        {
            var redirects = new Dictionary<String, String>();
            redirects = redirects.Concat(contents.ToDictionary(k => k.Id, v => v.Permalink))
                .GroupBy(k => k.Key)
                .ToDictionary(k => k.Key, v => v.First().Value);

            redirects = redirects.Concat(contents.ToDictionary(k => k.Slug, v => v.Permalink))
                .GroupBy(k => k.Key)
                .ToDictionary(k => k.Key, v => v.First().Value);

            return new RedirectsTemplateModel
            {
                Redirects = redirects
            };
        }

        public static String GetPagePermaLink(this IContentListItem contentListItem, Int32 page)
        {
            if (page == 1)
            {
                return contentListItem.Permalink;
            }

            if (contentListItem.Permalink.EndsWith("/"))
            {
                return $"{contentListItem.Permalink}page{page:00}";
            }

            return $"{contentListItem.Permalink}/page{page:00}";
        }

        public static IEnumerable<PostListTemplateModel> GetPages(this IContentListItem contentListItem, CrunchSite siteConfig)
        {
            var totalPages = (Int32)Math.Ceiling(contentListItem.Children.Count() / (Double)siteConfig.Pagination.PageSize);

            for (var i = 1; i <= totalPages; i++)
            {
                yield return new PostListTemplateModel(contentListItem, siteConfig, i, totalPages);
            }
        }

        public static SiteTemplateModel GetModel(this CrunchSite siteConfig)
        {
            return new SiteTemplateModel
            {
                Title = siteConfig.Title,
                SubTitle = siteConfig.SubTitle,
                Menu = siteConfig.Menu,
                Owner = siteConfig.Copyright.Owner,
                CopyrightYear = siteConfig.Copyright.StartYear
            };
        }
        public static SiteTemplateModel GetModel(this CrunchSite siteConfig, IContentProvider contentProvider)
        {
            return new SiteTemplateModel
            {
                Title = siteConfig.Title,
                SubTitle = siteConfig.SubTitle,
                Menu = siteConfig.Menu,
                Authors = siteConfig.Authors,
                Categories = contentProvider.Categories
                    .Select(c => siteConfig.Categories[c.Title.Split(':')[1].Trim()])
                    .OrderBy(c => c.Title)
                    .ToDictionary(k => k.Title, v => v),
                Tags = contentProvider.Tags
                    .Select(t => new TagMenuItem { Title = t.Title, Url = t.Permalink, Count = t.Children.Count() })
                    .OrderByDescending(t => t.Count)
                    .Take(20),
                Owner = siteConfig.Copyright.Owner,
                CopyrightYear = siteConfig.Copyright.StartYear
            };
        }

        public static PaginationListTemplateModel GetModel(this IDictionary<Int32, PaginationPageTemplateModel> pages, Int32 totalPages)
        {
            PaginationPageTemplateModel firstPage = null;
            PaginationPageTemplateModel lastPage = null;
            PaginationPageTemplateModel nextPage = null;
            PaginationPageTemplateModel prevPage = null;

            var currentPage = pages.Values.Where(p => p.IsCurrentPage).Single().Page;
            var pageSpan = 2;
            var startPage = 1;
            var endPage = totalPages;

            if (currentPage - pageSpan > 1)
            {
                startPage = currentPage - pageSpan;
                firstPage = pages[startPage - 1];
            }
            if (currentPage + pageSpan < totalPages)
            {
                endPage = currentPage + pageSpan;
                lastPage = pages[endPage + 1];
            }

            if (pages.ContainsKey(currentPage - 1))
            {
                prevPage = pages[currentPage - 1];
            }

            if (pages.ContainsKey(currentPage + 1))
            {
                nextPage = pages[currentPage + 1];
            }

            return new PaginationListTemplateModel
            {  // TODO: Fix the logic
                AllPages = pages,
                PageSpan = pages.Values.Skip(startPage - 1).Take((pageSpan * 2) + 1),
                FirstPage = firstPage,
                LastPage = lastPage,
                PreviousPage = prevPage,
                NextPage = nextPage,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };
        }
    }
}
