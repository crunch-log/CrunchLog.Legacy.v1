using System;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
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

            writer.WriteValue(file.ToRelative(_siteConfig.Paths.ContentPath));
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            //if (reader.Value is String fileKey && !String.IsNullOrWhiteSpace(fileKey))
            //{
            //    return ImageHelpers.GetImagePath(fileKey, _siteConfig.Paths.BasePath, _siteConfig.Paths.ImagesPath, _siteConfig.DefaultBanner);
            //}

            return _siteConfig.DefaultBanner;
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(DirectoryInfo);
        }
    }
}