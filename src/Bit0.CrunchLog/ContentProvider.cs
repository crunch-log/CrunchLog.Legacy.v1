using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Bit0.CrunchLog
{
    public class ContentProvider : IContentProvider
    {
        private readonly CrunchSite _siteConfig;
        private readonly ILogger<ContentProvider> _logger;

        private IDictionary<String, Content> _allContent;

        public ContentProvider(CrunchSite siteConfig, ILogger<ContentProvider> logger)
        {
            _logger = logger;
            _siteConfig = siteConfig;
        }

        public IDictionary<String, Content> AllContent
        {
            get
            {
                if (_allContent != null)
                {
                    return _allContent;
                }

                var allContent = new List<Content>();
                var files = _siteConfig.Paths.ContentPath.GetFiles("*.md", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    var pipeline = new MarkdownPipelineBuilder()
                        .UseYamlFrontMatter()
                        .Build();
                    var md = Markdown.Parse(file.GetText(), pipeline);
                    if (md[0] is YamlFrontMatterBlock)
                    {
                        try
                        {
                            var frontMatter = (md[0] as LeafBlock).Lines.ToString();

                            var deserializer = new DeserializerBuilder()
                                .WithNamingConvention(new CamelCaseNamingConvention())
                                .Build();
                            using (var stringReader = new StringReader(frontMatter))
                            {
                                var yaml = deserializer.Deserialize(stringReader);

                                var serializer = new SerializerBuilder()
                                    .JsonCompatible()
                                    .Build();

                                var json = serializer.Serialize(yaml);
                                var content = new Content(file, _siteConfig);

                                JsonConvert.PopulateObject(json, content);
                                allContent.Add(content);
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, $"Error reading front matter from: {file.FullName}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Skipping: {file}. Could not find front matter.");
                    }
                }


                _logger.LogDebug($"Found {allContent.Count} documents");

                _allContent = allContent
                    .OrderByDescending(x => x.DatePublished)
                    .ThenByDescending(x => x.Slug)
                    .ToDictionary(k => k.Id, v => v);

                return _allContent;
            }
        }

        public IEnumerable<Content> PublishedContent => AllContent.Where(p => p.Value.Published).Select(p => p.Value);

        public IEnumerable<Content> Posts => PublishedContent.Where(p => p.Layout == Layouts.Post);

        public IEnumerable<Content> Pages => PublishedContent.Where(p => p.Layout == Layouts.Page);

        public IEnumerable<ContentListItem> PostTags => Posts
                    .Where(p => p.Tags != null && p.Tags.Any())
                    .SelectMany(p => p.Tags)
                    .GroupBy(t => t.Key)
                    .Select(t => t.First().Value)
                    .Select(t => new ContentListItem
                    {
                        Title = t.Title,
                        Permalink = t.Permalink,
                        Layout = Layouts.Tag,
                        Children = Posts.Where(p => p.Tags.Keys.Contains(t.Title))
                    });

        public IEnumerable<ContentListItem> PostCategories => Posts
                    .Where(p => p.Categories != null && p.Categories.Any())
                    .SelectMany(p => p.Categories)
                    .GroupBy(c => c.Key)
                    .Select(c => c.First().Value)
                    .Select(c => new ContentListItem
                    {
                        Title = c.Title,
                        Permalink = c.Permalink,
                        Layout = Layouts.Category,
                        Children = Posts.Where(p => p.Categories.Keys.Contains(c.Title))
                    });

        public IEnumerable<ContentListItem> Authors => Posts
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
