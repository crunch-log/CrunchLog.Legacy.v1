using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bit0.CrunchLog
{
    public class ContentProvider : IContentProvider
    {
        private readonly CrunchSite _siteConfig;
        private readonly ILogger<ContentProvider> _logger;

        private IEnumerable<Content> _allContent;

        public ContentProvider(CrunchSite siteConfig, ILogger<ContentProvider> logger)
        {
            _logger = logger;
            _siteConfig = siteConfig;
        }

        public IEnumerable<Content> AllContent
        {
            get
            {
                if (_allContent != null)
                {
                    return _allContent;
                }

                var allContent = new List<Content>();

                var metaFiles = _siteConfig.Paths.ContentPath.GetFiles("*.json", SearchOption.AllDirectories);
                foreach (var metaFile in metaFiles)
                {
                    var content = new Content(metaFile, _siteConfig);

                    JsonConvert.PopulateObject(metaFile.OpenText().ReadToEnd(), content);
                    allContent.Add(content);
                }

                _logger.LogDebug($"Fount {allContent.Count} documents");

                _allContent = allContent
                    .OrderByDescending(x => x.Date)
                    .ThenByDescending(x => x.Slug);

                return _allContent;
            }
        }

        public IEnumerable<Content> PublishedContent
        {
            get
            {
                return AllContent.Where(p => p.Published);
            }
        }

        public IEnumerable<Content> Posts
        {
            get
            {
                return PublishedContent.Where(p => p.Layout == Layouts.Post);
            }
        }

        public IEnumerable<Content> Pages
        {
            get
            {
                return PublishedContent.Where(p => p.Layout == Layouts.Page);
            }
        }

        public IEnumerable<ContentListItem> PostTags
        {
            get
            {
                return Posts
                    .Where(p => p.Tags != null && p.Tags.Any())
                    .SelectMany(p => p.Tags)
                    .Distinct()
                    .Select(t => new ContentListItem
                    {
                        Title = t.Key,
                        Permalink = t.Value,
                        Layout = Layouts.Tag,
                        Children = Posts.Where(p => p.Tags.Contains(t))
                    });
            }
        }

        public IEnumerable<ContentListItem> PostCategories
        {
            get
            {
                return Posts
                    .Where(p => p.Categories != null && p.Categories.Any())
                    .SelectMany(p => p.Categories)
                    .Distinct()
                    .Select(c => new ContentListItem
                    {
                        Title = c.Key,
                        Permalink = c.Value,
                        Layout = Layouts.Category,
                        Children = Posts.Where(p => p.Categories.Contains(c))
                    });
            }
        }

        public IEnumerable<ContentListItem> Authors
        {
            get
            {
                return Posts
                    .Where(p => p.Author != null)
                    .Select(p => p.Author)
                    .Distinct()
                    .Select(a => new ContentListItem
                    {
                        Title = a.Name,
                        Permalink = a.Permalink,
                        Layout = Layouts.Author,
                        Children = Posts.Where(p => p.Author.Alias.Equals(a.Alias, StringComparison.InvariantCultureIgnoreCase))
                    });
            }
        }

        public IEnumerable<ContentListItem> PostArchives
        {
            get
            {
                var archives = new List<ContentListItem>();

                var permaLinks = Posts
                    .Select(p => p.Permalink.Split('/'))
                    .ToList();

                var years = permaLinks
                    .Select(x => x[1])
                    .Distinct();

                foreach (var year in years)
                {
                    var ySlug = $"/{year}/";
                    archives.Add(new ContentListItem
                    {
                        Title = ySlug,
                        Permalink = ySlug,
                        Layout = Layouts.Archive,
                        Children = Posts.Where(p => p.Permalink.StartsWith(ySlug))
                    });

                    var months = permaLinks
                        .Where(x => x[1] == year)
                        .Select(x => x[2])
                        .Distinct();

                    foreach (var month in months)
                    {
                        var mSlug = $"/{year}/{month}/";
                        archives.Add(new ContentListItem
                        {
                            Title = mSlug,
                            Permalink = mSlug,
                            Layout = Layouts.Archive,
                            Children = Posts.Where(p => p.Permalink.StartsWith(mSlug))
                        });
                    }
                }

                return archives.OrderBy(a => a.Permalink);
            }
        }

        public ContentListItem Home => new ContentListItem
        {
            Layout = Layouts.Home,
            Permalink = "/",
            Title = "Home",
            Children = Posts
        };

        public IDictionary<String, IContent> Links
        {
            get
            {
                var dict = new Dictionary<String, IContent>
                {
                    { "/", Home }
                };

                foreach (var content in PublishedContent)
                {
                    dict.Add(content.Permalink, content);
                }

                foreach (var archive in PostArchives)
                {
                    dict.Add(archive.Permalink, archive);
                }

                foreach (var tags in PostTags)
                {
                    dict.Add(tags.Permalink, tags);
                }

                foreach (var category in PostCategories)
                {
                    dict.Add(category.Permalink, category);
                }

                foreach (var author in Authors)
                {
                    dict.Add(author.Permalink, author);
                }

                return dict.OrderBy(l => l.Key).ToDictionary(k => k.Key, v => v.Value);
            }
        }
    }
}
