using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.ContentTypes;
using Bit0.CrunchLog.Extensions;
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

        public IDictionary<FileInfo, TContent> GetAll<TContent>() where TContent : IContent, new()
        {
            var list = new Dictionary<FileInfo, TContent>();

            var contentType = typeof(TContent).GetCustomAttribute<ContentTypeAttribute>();
            var type = contentType.ContentTypeName;

            var contentPath = new DirectoryInfo(_config.BasePath.CombinePath(_config.Paths[type]));

            foreach (var metaFile in contentPath.GetFiles("*.json", SearchOption.AllDirectories))
            {
                var content = new TContent
                {
                    MetaFile = metaFile,
                    PermaLink = _config.Permalinks[type]
                };
                _jsonSerializer.Populate(metaFile?.OpenText(), content);

                list.Add(metaFile, content);
            }

            return list;
        }
    }
}
