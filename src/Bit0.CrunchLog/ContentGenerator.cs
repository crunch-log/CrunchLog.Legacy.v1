using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Factory;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Bit0.CrunchLog
{
    public class ContentGenerator : IContentGenerator
    {
        private readonly IContentProvider _contentProvider;
        private readonly ITemplateFactory _templateFactory;
        private readonly CrunchSite _siteConfig;
        private readonly ILogger<ContentGenerator> _logger;

        public ContentGenerator(IContentProvider contentProvider,
            ITemplateFactory templateFactory,
            CrunchSite siteConfig,
            ILogger<ContentGenerator> logger)
        {
            _templateFactory = templateFactory;
            _logger = logger;
            _siteConfig = siteConfig;
            _contentProvider = contentProvider;
        }

        public void CleanOutput()
        {
            if (_siteConfig.Paths.OutputPath.Exists)
            {
                _siteConfig.Paths.OutputPath.ClearFolder();
                _logger.LogInformation($"Cleaned output folder {_siteConfig.Paths.OutputPath.FullName}");
            }
        }

        public void PublishCategories()
        {
            var pages = _contentProvider.PostCategories.SelectMany(archive => archive.GetPages(_siteConfig)).ToList();

            foreach (var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogInformation($"Categories published in {pages.Count} pages");
        }

        public void PublishTags()
        {
            var pages = _contentProvider.PostTags.SelectMany(archive => archive.GetPages(_siteConfig)).ToList();

            foreach (var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogInformation($"Tags published in {pages.Count} pages");
        }

        public void PublishArchive()
        {
            var pages = _contentProvider.PostArchives.SelectMany(archive => archive.GetPages(_siteConfig)).ToList();

            foreach (var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogInformation($"Archives published in {pages.Count} pages");
        }

        public void PublisHome()
        {
            var pages = _contentProvider.Home.GetPages(_siteConfig).ToList();

            foreach (var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogInformation($"Home published in {pages.Count} pages");
        }

        public void PublishContent()
        {
            var published = _contentProvider.PublishedContent.ToList();

            foreach (var content in published)
            {
                _templateFactory.Render(content.GetModel());
            }

            _logger.LogInformation($"Published {published.Count} posts/pages");
        }

        public void PublishImages()
        {
            _siteConfig.Paths.ImagesPath.Copy(_siteConfig.Paths.OutputPath.CombineDirPath("images"));
        }

        public void PublishAuthors()
        {
            var authors = _contentProvider.Authors.SelectMany(author => author.GetPages(_siteConfig)).ToList();

            foreach (var author in authors)
            {
                _templateFactory.Render(author);
            }

            _logger.LogInformation($"Authors published in {authors.Count} pages");
        }

        public void PublishSiteInfo()
        {
            var info = _siteConfig.GetModel(_contentProvider);
            _templateFactory.Render(info);

            _logger.LogInformation($"Site information published");
        }

        public void PublishRedirectsList()
        {
            var model = _contentProvider.PublishedContent.GetRedirectModel();
            _templateFactory.Render(model);

            _logger.LogInformation("Site redirects published");
        }

        public void PublishAssets()
        {
            _siteConfig.Paths.AssetsPath.Copy(_siteConfig.Paths.OutputPath);
        }

        public void Publish()
        {
            _templateFactory.PreProcess();

            PublishSiteInfo();
            PublishRedirectsList();
            PublishImages();
            PublishAssets();
            PublisHome();
            PublishContent();
            PublishArchive();
            PublishCategories();
            PublishTags();
            PublishAuthors();

            _templateFactory.PostProcess();
        }
    }
}
