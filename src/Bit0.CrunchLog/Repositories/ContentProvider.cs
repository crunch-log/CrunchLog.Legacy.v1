using System.Collections.Generic;
using System.IO;
using Bit0.CrunchLog.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContentProvider : IContentProvider
    {
        private readonly CrunchConfig _config;
        private readonly JsonSerializer _jsonSerializer;
        private readonly ILogger<ContentProvider> _logger;

        public ContentProvider(JsonSerializer jsonSerializer, CrunchConfig config, ILogger<ContentProvider> logger)
        {
            _logger = logger;
            _config = config;
            _jsonSerializer = jsonSerializer;
        }

        public IDictionary<FileInfo, Content> GetAll()
        {
            var list = new Dictionary<FileInfo, Content>();
            
            foreach (var metaFile in _config.BasePath.GetFiles("*.json", SearchOption.AllDirectories))
            {
                var content = new Content
                {
                    MetaFile = metaFile,
                    PermaLink = _config.Permalink
                };

                _jsonSerializer.Populate(metaFile?.OpenText(), content);

                list.Add(metaFile, content);
            }

            _logger.LogDebug($"Fount {list.Count} documents");

            return list;
        }
    }
}
