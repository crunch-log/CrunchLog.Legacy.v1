using System;
using System.IO;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.JsonConverters
{
    public class BannerImageConverter : JsonConverter
    {
        private readonly CrunchSite _siteConfig;

        public BannerImageConverter()
        {
            _siteConfig = ServiceProviderFactory.Current.GetService<CrunchSite>();
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var file = (FileInfo)value;

            writer.WriteValue(file.FullName.Replace(_siteConfig.Paths.BasePath.FullName, "").NormalizePath());
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var fileKey = (String)reader.Value;

            if (String.IsNullOrWhiteSpace(fileKey))
            {
                // return defualt
            }

            return null; // get fileInfo from key
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(DirectoryInfo);
        }
    }
}