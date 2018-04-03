using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.TemplateModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bit0.CrunchLog
{
    public interface IContentProvider
    {
        IEnumerable<Content> AllContent { get; }
        IEnumerable<Content> PublishedContent { get; }
        IEnumerable<Content> Posts { get; }
        IEnumerable<TagTemplateModel> PostTags { get; }
        IEnumerable<CategoryTemplateModel> PostCategories { get; }
        IEnumerable<ArchiveTemplateModel> PostArchives { get; }

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
                    content.Fix(_config);
                    allContent.Add(content);
                }

                _logger.LogDebug($"Fount {allContent.Count} documents");

                _allContent = allContent;

                return _allContent;
            }
        }

        public IEnumerable<Content> PublishedContent
        {
            get
            {
                return AllContent
                    .Where(x => x.Published);
            }
        }

        public IEnumerable<Content> Posts
        {
            get
            {
                return PublishedContent
                    .Where(x => x.Layout == Layouts.Post)
                    .OrderByDescending(x => x.Date)
                    .ThenByDescending(x => x.Slug);
            }
        }

        public IEnumerable<TagTemplateModel> PostTags
        {
            get
            {
                return Posts
                    .Where(x => x.Tags != null)
                    .SelectMany(x => x.Tags)
                    .Distinct()
                    .Select(x => new TagTemplateModel
                    {
                        Name = x,
                        Permalink = $"/tag/{x}",
                        Posts = Posts
                            .Where(p => p.Tags.Contains(x))
                            .Select(p => new PostTemplateModel(_config, p))
                    });
            }
        }

        public IEnumerable<CategoryTemplateModel> PostCategories
        {
            get
            {
                return Posts
                    .Where(x => x.Categories != null)
                    .SelectMany(x => x.Categories)
                    .Distinct()
                    .Select(x => new CategoryTemplateModel
                    {
                        Name = x,
                        Permalink = $"/category/{x}",
                        Posts = Posts
                            .Where(p => p.Categories.Contains(x))
                            .Select(p => new PostTemplateModel(_config, p))
                    });
            }
        }

        public IEnumerable<ArchiveTemplateModel> PostArchives
        {
            get
            {
                var archives = new List<ArchiveTemplateModel>();

                var permaLinks = Posts
                    .Select(x => x.Permalink.Split('/'))
                    .ToList();

                var years = permaLinks
                    .Select(x => x[1])
                    .Distinct();

                foreach (var year in years)
                {
                    var ySlug = $"/{year}/";
                    archives.Add(new ArchiveTemplateModel
                    {
                        Name = ySlug,
                        Permalink = ySlug,
                        Posts = Posts
                            .Where(x => x.Permalink.StartsWith(ySlug))
                            .Select(p => new PostTemplateModel(_config, p))
                    });

                    var months = permaLinks
                        .Where(x => x[1] == year)
                        .Select(x => x[2])
                        .Distinct();

                    foreach (var month in months)
                    {
                        var mSlug = $"/{year}/{month}/";
                        archives.Add(new ArchiveTemplateModel
                        {
                            Name = mSlug,
                            Permalink = mSlug,
                            Posts = Posts
                                .Where(x => x.Permalink.StartsWith(mSlug))
                                .Select(p => new PostTemplateModel(_config, p))
                        });
                    }
                }

                return archives;
            }
        }
    }
}
