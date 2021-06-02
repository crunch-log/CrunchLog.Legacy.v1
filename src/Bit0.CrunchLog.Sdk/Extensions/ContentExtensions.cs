using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Helpers;
using Bit0.CrunchLog.Template.Models;
using Bit0.CrunchLog.Template.Models.MetaData;

namespace Bit0.CrunchLog.Extensions
{
    public static class ContentExtensions
    {
        public static PostTemplateModel GetModel(this IContent content, CrunchConfig siteConfig, Boolean inList = false)
        {
            return new PostTemplateModel(content, siteConfig, inList);
        }

        public static IEnumerable<PostRedirectTemplateModel> GetRedirectModels(this IContent content, CrunchConfig siteConfig)
        {
            foreach(var url in content.Redirects)
            {
                yield return new PostRedirectTemplateModel(content, siteConfig, url);
            }
        }

        public static String GetPagePermaLink(this IContentListItem contentListItem, Int32 page)
        {
            if(page == 1)
            {
                return contentListItem.Permalink;
            }

            if(contentListItem.Permalink.EndsWith("/"))
            {
                return $"{contentListItem.Permalink}page{page:00}";
            }

            return $"{contentListItem.Permalink}/page{page:00}";
        }

        public static IEnumerable<PostListTemplateModel> GetPages(this IContentListItem contentListItem, CrunchConfig siteConfig)
        {
            var totalPages = (Int32)Math.Ceiling(contentListItem.Children.Count() / (Double)siteConfig.Pagination.PageSize);

            for(var i = 1; i <= totalPages; i++)
            {
                yield return new PostListTemplateModel(contentListItem, siteConfig, i, totalPages);
            }
        }

        public static ListMetaData GetMetaData(this IContentListItem contentListItem, CrunchConfig siteConfig, IEnumerable<ITemplateModel> posts)
        {
            var image = siteConfig.DefaultBannerImage;
            var description = siteConfig.Description;
            var tags = siteConfig.Tags.Keys as IEnumerable<String>;

            if(contentListItem.Layout == Layouts.Category)
            {
                var category = GetCategory(siteConfig, contentListItem.Name);
                description = category.Description;
                image = category.Image;
                tags = new[] { category.Title }.Concat(tags);
            }

            if(contentListItem.Layout == Layouts.Tag)
            {
                tags = new[] { contentListItem.Name }.Concat(tags);
            }

            image ??= siteConfig.DefaultBannerImage;

            var publishedDate = posts.Cast<PostTemplateModel>().Max(p => p.Published);
            var updatedDate = posts.Cast<PostTemplateModel>().Max(p => p.Updated);

            return new ListMetaData
            {
                Title = contentListItem.Title,
                PublishedDate = publishedDate,
                UpdatedDate = updatedDate,
                Category = contentListItem.Title,
                ShortUrl = contentListItem.Permalink,
                Archive = new ArchiveMetaData
                {
                    Text = contentListItem.Title,
                    Url = contentListItem.Permalink,
                },
                Type = contentListItem.Layout.GetValue(),
                CanonicalUrl = siteConfig.BaseUrl,
                Language = siteConfig.LanguageCode,
                Image = image,
                Robots = String.Join(", ", siteConfig.Robots),
                Description = description,
                Keywords = String.Join(", ", tags)
            };
        }

        public static PostMetaData GetMetaData(this IContent content, CrunchConfig siteConfig)
        {
            var archive = content.DatePublished.ToString(@"\/yyyy\/MM\/");

            return new PostMetaData
            {
                Title = content.Title,
                Description = content.Intro,
                Keywords = String.Join(", ", content.Tags.Keys),
                PublishedDate = content.DatePublished,
                UpdatedDate = content.DateUpdated,
                Category = content.DefaultCategory.Title,
                ShortUrl = content.ShortUrl,
                Archive = new ArchiveMetaData
                {
                    Text = archive,
                    Url = archive
                },
                Type = content.Layout.GetValue(),
                CanonicalUrl = content.Permalink,
                Language = siteConfig.LanguageCode,
                Image = content.Image,
            };
        }

        public static SiteMetaData GetMetaData(this CrunchConfig siteConfig)
        {
            return new SiteMetaData
            {
                Title = siteConfig.Title,
                SubTitle = siteConfig.SubTitle,
                Designer = siteConfig.Theme.Author.ToString(),
                Copyright = siteConfig.Copyright.ToString(),
                Description = siteConfig.Description,
                Logo = siteConfig.Logo,
                Manifest = "/manifest.json",
                ThemeColor = siteConfig.Manifest?.ThemeColor,
                CanonicalUrl = siteConfig.BaseUrl,
                BaseUrl = siteConfig.BaseUrl,
                Social = siteConfig.Social,
                Language = siteConfig.LanguageCode,
                Icon = new IconMetaData
                {
                    Favicon = siteConfig.Icons?["favicon"]?.Url,
                    Favicon16 = siteConfig.Icons?["favicon16"]?.Url,
                    Favicon32 = siteConfig.Icons?["favicon32"]?.Url,
                    Favicon144 = siteConfig.Icons?["favicon144"]?.Url,
                    Favicon152 = siteConfig.Icons?["favicon152"]?.Url,
                    Favicon192 = siteConfig.Icons?["favicon192"]?.Url,
                    Favicon512 = siteConfig.Icons?["favicon512"]?.Url,
                    PinSvg = siteConfig.Icons?["pinSvg"]?.Url
                },
            };
        }

        public static SiteTemplateModel GetModel(this CrunchConfig siteConfig)
        {
            return new SiteTemplateModel
            {
                Title = siteConfig.Title,
                SubTitle = siteConfig.SubTitle,
                Logo = siteConfig.Logo,
                Menu = siteConfig.Menu,
                Owner = siteConfig.Copyright.Owner,
                CopyrightYear = siteConfig.Copyright.StartYear
            };
        }
        public static SiteTemplateModel GetModel(this CrunchConfig siteConfig, IContentProvider contentProvider)
        {
            return new SiteTemplateModel
            {
                Title = siteConfig.Title,
                SubTitle = siteConfig.SubTitle,
                Logo = siteConfig.Logo,
                Menu = siteConfig.Menu,
                Authors = siteConfig.Authors,
                Categories = contentProvider.Categories
                    .Select(c => GetCategory(siteConfig, c.Name))
                    .OrderBy(c => c.Title)
                    .ToDictionary(k => k.Title, v => v),
                Tags = contentProvider.Tags
                    .Select(t => new TagMenuItem { Title = t.Title, Url = t.Permalink, Count = t.Children.Count() })
                    .OrderByDescending(t => t.Count)
                    .Take(20),
                Owner = siteConfig.Copyright.Owner,
                CopyrightYear = siteConfig.Copyright.StartYear,
                Meta = siteConfig.GetMetaData()
            };
        }

        private static CategoryInfo GetCategory(CrunchConfig siteConfig, String catName)
        {
            if(!siteConfig.Categories.ContainsKey(catName))
            {

                var defaultCat = siteConfig.Categories[siteConfig.DefaultCategory];

                var cat = new CategoryInfo
                {
                    Title = catName,
                    Permalink = String.Format(StaticKeys.CategoryPathFormat, catName),
                    Color = defaultCat.Color,
                    ShowInMainMenu = false
                };

                siteConfig.Categories.Add(catName, cat);
            }

            return siteConfig.Categories[catName];
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

            if(currentPage - pageSpan > 1)
            {
                startPage = currentPage - pageSpan;
                firstPage = pages[startPage - 1];
            }
            if(currentPage + pageSpan < totalPages)
            {
                endPage = currentPage + pageSpan;
                lastPage = pages[endPage + 1];
            }

            if(pages.ContainsKey(currentPage - 1))
            {
                prevPage = pages[currentPage - 1];
            }

            if(pages.ContainsKey(currentPage + 1))
            {
                nextPage = pages[currentPage + 1];
            }

            return new PaginationListTemplateModel
            {
                AllPages = pages,
                PageSpan = pages.Values.Skip(startPage - 1).Take(( pageSpan * 2 ) + 1),
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
