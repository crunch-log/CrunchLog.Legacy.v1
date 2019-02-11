using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    return array.ToObject<IList<String>>()
                        .ToDictionary(k => k, v => new CategoryInfo
                        {
                            Title = v,
                            Permalink = String.Format(StaticKeys.TagPathFormat, v),
                        });
                case Layouts.Category:
                    return array.ToObject<IList<String>>().ToDictionary(k => k, v =>
                    {
                        var defCat = config.Categories[config.DefaultCategory];

                        return config.Categories.ContainsKey(v) 
                        ? config.Categories[v]
                        : new CategoryInfo
                        {
                            Title = v,
                            Permalink = String.Format(StaticKeys.CategoryPathFormat, v),
                            Color = defCat.Color,
                            Image = defCat.Color
                        };
                    });
                default:
                    break;
            }

            return null;
        }

        public override Boolean CanConvert(Type objectType) => objectType == typeof(IDictionary<String, String>);
    }
}