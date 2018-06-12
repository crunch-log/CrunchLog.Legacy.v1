using System;
using System.IO;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.JsonConverters
{
    public class PathConverter : JsonConverter
    {
        private readonly String _pathKey;

        public PathConverter(String pathKey) : base()
        {
            _pathKey = pathKey;
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var dir = (DirectoryInfo)value;

            writer.WriteValue(dir.Name);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var configFile = ServiceProviderFactory.Current.GetService<ConfigFile>();
            var basePath = configFile.File.Directory;
            
            var pathKey = (String)reader.Value;

            if (String.IsNullOrWhiteSpace(pathKey))
            {
                pathKey = _pathKey;
            }

            return basePath.CombineDirPath(pathKey.NormalizePath());
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(Author);
        }
    }
}