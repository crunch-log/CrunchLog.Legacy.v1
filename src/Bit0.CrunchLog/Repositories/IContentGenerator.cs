using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.ContentTypes;
using Bit0.CrunchLog.Extensions;
using Microsoft.Extensions.Logging;

namespace Bit0.CrunchLog.Repositories
{
    public interface IContentGenerator
    {
        void PublishAll<TContent>() where TContent : IContent, new();
        void CleanOutput();
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContentGenerator : IContentGenerator
    {
        private readonly IContentProvider _contentProvider;
        private readonly CrunchConfig _config;
        private readonly ILogger<ContentGenerator> _logger;

        public ContentGenerator(IContentProvider contentProvider, CrunchConfig config, ILogger<ContentGenerator> logger)
        {
            _logger = logger;
            _config = config;
            _contentProvider = contentProvider;
        }

        public void CleanOutput()
        {
            if (_config.OutputPath.Exists)
            {
                _config.OutputPath.Delete(true);
            }

            _logger.LogInformation($"Cleaned output folder {_config.OutputPath.FullName}");

        }

        public void PublishAll<TContent>() where TContent : IContent, new()
        {
            var all = _contentProvider.GetAll<TContent>().ToList();
            var published = all.Where(c => c.Value.Published).ToList();

            foreach (var content in published)
            {
                content.Value.WriteFile(_config.OutputPath);
            }

            _logger.LogInformation($"Published {typeof(TContent).Name}: {published.Count}/{all.Count}");
        }
    }
}
