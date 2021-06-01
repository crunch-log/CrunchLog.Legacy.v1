using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Template.Factory;
using Bit0.CrunchLog.Template.Models;
using Microsoft.Extensions.Logging;

namespace Bit0.CrunchLog
{
    public class ContentGenerator : IContentGenerator
    {
        private readonly IContentProvider _contentProvider;
        private readonly ITemplateFactory _templateFactory;
        private readonly CrunchConfig _siteConfig;
        private readonly ILogger<IContentGenerator> _logger;

        public ContentGenerator(IContentProvider contentProvider,
            ITemplateFactory templateFactory,
            CrunchConfig siteConfig,
            ILogger<IContentGenerator> logger)
        {
            _templateFactory = templateFactory;
            _logger = logger;
            _siteConfig = siteConfig;
            _contentProvider = contentProvider;
        }

        public void CleanOutput()
        {
            if(_siteConfig.Paths.OutputPath.Exists)
            {
                _siteConfig.Paths.OutputPath.ClearFolder();
                _logger.LogDebug($"Cleaned output folder {_siteConfig.Paths.OutputPath.FullName}");
            }
        }

        public void PublishCategories()
        {
            var pages = _contentProvider.Categories.SelectMany(archive => archive.GetPages(_siteConfig)).ToList();

            foreach(var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogDebug($"Categories published in {pages.Count} pages");
        }

        public void PublishTags()
        {
            var pages = _contentProvider.Tags.SelectMany(archive => archive.GetPages(_siteConfig)).ToList();

            foreach(var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogDebug($"Tags published in {pages.Count} pages");
        }

        public void PublishArchive()
        {
            var pages = _contentProvider.PostArchives.SelectMany(archive => archive.GetPages(_siteConfig)).ToList();

            foreach(var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogDebug($"Archives published in {pages.Count} pages");
        }

        public void PublisHome()
        {
            var pages = _contentProvider.Home.GetPages(_siteConfig).ToList();

            foreach(var page in pages)
            {
                _templateFactory.Render(page);
            }

            _logger.LogDebug($"Home published in {pages.Count} pages");
        }

        public void PublishContent()
        {
            var published = _contentProvider.PublishedContent.ToList();

            foreach(var content in published)
            {
                _templateFactory.Render(content.GetModel(_siteConfig));
            }

            _logger.LogDebug($"Published {published.Count} posts/pages");
        }

        public void PublishContentRedirects()
        {
            var redirects = _contentProvider.PublishedContent.SelectMany(c => c.GetRedirectModels(_siteConfig)).ToList();

            foreach(var redirect in redirects)
            {
                _templateFactory.Render(redirect);
            }

            _logger.LogDebug($"Published {redirects.Count} redirects.");
        }

        public void PublishImages()
        {
            _siteConfig.Paths.ImagesPath.Copy(_siteConfig.Paths.OutputPath.CombineDirPath("images"));
            _logger.LogDebug("Site images published");
        }

        public void PublishAuthors()
        {
            var authors = _contentProvider.Authors.SelectMany(author => author.GetPages(_siteConfig)).ToList();

            foreach(var author in authors)
            {
                _templateFactory.Render(author);
            }

            _logger.LogDebug($"Authors published in {authors.Count} pages");
        }

        public void PublishSiteInfo()
        {
            var info = _siteConfig.GetModel(_contentProvider);
            _templateFactory.Render(info);

            _logger.LogDebug($"Site information published");
        }

        public void PublishAssets()
        {
            _siteConfig.Paths.AssetsPath.Copy(_siteConfig.Paths.OutputPath);
            _logger.LogDebug("Site assets published");
        }

        public void PublishDrafts()
        {
            var drafts = _contentProvider.DraftContent.ToList();

            foreach(var content in drafts)
            {
                _templateFactory.Render(content.GetModel(_siteConfig));
            }

            _logger.LogDebug($"Published {drafts.Count} drafts.");
        }

        public void Publish404()
        {
            var model = new NotFoundTemplateModel();
            _templateFactory.Render(model);

            _logger.LogDebug("Site 404 published");
        }

        public void Publish()
        {
            _templateFactory.PreProcess();

            PublishSiteInfo();
            PublishImages();
            PublishAssets();
            PublisHome();
            PublishContent();
            PublishContentRedirects();
            PublishDrafts();
            PublishArchive();
            PublishCategories();
            PublishTags();
            PublishAuthors();
            Publish404();

            _templateFactory.PostProcess();
        }
    }
}
