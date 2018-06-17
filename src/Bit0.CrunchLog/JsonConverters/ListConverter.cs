using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Bit0.CrunchLog.JsonConverters
{
    public class ListConverter : JsonConverter
    {
        private readonly Layouts _layoutKey;

        public ListConverter(Layouts layoutType) : base()
        {
            _layoutKey = layoutType;
        }

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var dict = (IDictionary<String, String>)value;

            writer.WriteValue(dict.Keys);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            //var word = (String)reader.Value;
            var config = ServiceProviderFactory.Current.GetService<CrunchSite>();

            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception($"'{reader.Path}' needs to be a array");
            }

            var array = JArray.Load(reader);

            switch (_layoutKey)
            {
                case Layouts.Tag:
                    return config.Tags.Concat(array.ToObject<IList<String>>()).ToDictionary(k => k, v => $"/tag/{v}/");
                case Layouts.Category:
                    return array.ToObject<IList<String>>().ToDictionary(k => k, v => $"/category/{v}/");
                default:
                    break;
            }
            
            return null;
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(IDictionary<String, String>);
        }
    }
}