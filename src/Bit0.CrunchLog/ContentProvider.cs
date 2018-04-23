using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bit0.CrunchLog
{
    public interface IContentProvider
    {
        IEnumerable<Content> AllContent { get; }
        IEnumerable<Content> PublishedContent { get; }
        IEnumerable<Content> Posts { get; }
        IEnumerable<Content> Pages { get; }
        IEnumerable<ContentListItem> PostTags { get; }
        IEnumerable<ContentListItem> PostCategories { get; }
        IEnumerable<ContentListItem> PostArchives { get; }
        IDictionary<String, IContent> Links { get; }
        ContentTreeNode Tree { get; }
        IDictionary<String, ContentTreeNode> TreeLinks { get; }
    }

    public class ContentListItem : IContent
    {
        public String Layout { get; set; }
        public String Permalink { get; set; }
        public String Title { get; set; }
        public IEnumerable<IContent> Children { get; set; }
    }

    public class ContentTreeNode
    {
        public String Permalink { get; set; }
        public IContent Parent { get; set; }
        public IContent Current { get; set; }
        public IList<ContentTreeNode> Children { get; set; } = new List<ContentTreeNode>();

        public override String ToString()
        {
            return $"{Permalink}, Children: {Children.Count}";
        }
    }

    public class EmptyContent : IContent
    {
        public String Layout { get; set; } = Layouts.Empty;
        public String Permalink { get; set; } = "";
        public String Title { get; set; } = "{EMPTY}";
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContentProvider : IContentProvider
    {
        private readonly CrunchConfig _config;
        private readonly JsonSerializer _jsonSerializer;
        private readonly ILogger<ContentProvider> _logger;

        private IEnumerable<Content> _allContent;

        public ContentProvider(JsonSerializer jsonSerializer, CrunchConfig config, ILogger<ContentProvider> logger)
        {
            _logger = logger;
            _config = config;
            _jsonSerializer = jsonSerializer;
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

                var metaFiles = _config.Paths.ContentPath.GetFiles("*.json", SearchOption.AllDirectories);
                foreach (var metaFile in metaFiles)
                {
                    var content = new Content(metaFile, _config.Permalink);

                    _jsonSerializer.Populate(metaFile.OpenText(), content);
                    content.UpdateProperties(_config);
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
                        Title = t,
                        Permalink = $"/tag/{t}",
                        Layout = Layouts.Tags,
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
                        Title = c,
                        Permalink = $"/category/{c}",
                        Layout = Layouts.Category,
                        Children = Posts.Where(p => p.Categories.Contains(c))
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

        private ContentListItem Home => new ContentListItem
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

                return dict.OrderBy(l => l.Key).ToDictionary(k => k.Key, v => v.Value);
            }
        }

        public IDictionary<String, ContentTreeNode> TreeLinks { get; } = new Dictionary<String, ContentTreeNode>();

        public ContentTreeNode Tree
        {
            get
            {
                // add home
                // add pages
                // add posts
                // add archives
                // add categories
                // add tags
                Links.Keys.ToList().ForEach(x => { AddNode(x); });
                return TreeLinks.FirstOrDefault().Value;
            }
        }

        private ContentTreeNode AddNode(String path)
        {
            var segments = path.Split('/').Where(s => !String.IsNullOrWhiteSpace(s)).ToArray();
            if (!segments.Any())
            {
                if (!path.Equals("/", StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                var home = new ContentTreeNode
                {
                    Current = Links[path],
                    Parent = null,
                    Permalink = path
                };
                TreeLinks.Add(path, home);

                return home;
            }

            var parent = $"/{String.Join("/", segments.Reverse().Skip(1).Reverse())}/".Replace("//", "/");
            var parentNode = TreeLinks.FirstOrDefault(n => n.Key == parent).Value
                             ?? AddNode(parent);

            IContent content;
            if (Links.ContainsKey(path))
            {
                content = Links[path];
            }
            else
            {
                if (path.Equals("/category/", StringComparison.InvariantCultureIgnoreCase))
                {
                    content = new ContentListItem
                    {
                        Permalink = path,
                        Layout = Layouts.Category,
                        Title = "Categories",
                        Children = PostCategories
                    };
                }
                else if (path.Equals("/tag/", StringComparison.InvariantCultureIgnoreCase))
                {
                    content = new ContentListItem
                    {
                        Permalink = path,
                        Layout = Layouts.Tags,
                        Title = "Tags",
                        Children = PostTags
                    };
                }
                else
                {
                    content = new EmptyContent();
                }
            }

            var node = new ContentTreeNode
            {
                Current = content,
                Parent = parentNode.Current,
                Permalink = path
            };

            parentNode.Children.Add(node);
            TreeLinks.Add(path, node);
            return node;
        }
    }
}
