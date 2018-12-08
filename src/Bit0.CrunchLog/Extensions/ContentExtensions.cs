using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Template.Models;

namespace Bit0.CrunchLog.Extensions
{
    public static class ContentExtensions
    {
        public static PostTemplateModel GetModel(this Content content, Boolean inList = false)
        {
            return new PostTemplateModel(content, inList); 
        }

        public static RedirectsTemplateModel GetRedirectModel(this IEnumerable<Content> contents)
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

        public static String GetPagePermaLink(this ContentListItem contentListItem, Int32 page)
        {
            if (page == 1)
            {
                return contentListItem.Permalink;
            }

            if (contentListItem.Permalink.EndsWith("/"))
            {
                return $"{contentListItem.Permalink}page{page:00}";
            }

            return  $"{contentListItem.Permalink}/page{page:00}";
        }

        public static IEnumerable<PostListTemplateModel> GetPages(this ContentListItem contentListItem, CrunchSite siteConfig)
        {
            var totalPages = (Int32) Math.Ceiling(contentListItem.Children.Count() / (Double) siteConfig.Pagination.PageSize);

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
    }
}
