using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Helpers;
using Bit0.CrunchLog.Template.Models;
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
                    var postTags = array.ToObject<IList<String>>()
                        .ToDictionary(k => k, v => new CategoryInfo
                        {
                            Title = v,
                            Permalink = String.Format(StaticKeys.TagPathFormat, v),
                        });

                    return config.Tags
                        .Concat(postTags)
                        .GroupBy(k => k.Key)
                        .ToDictionary(k => k.Key, v => v.First().Value);
                case Layouts.Category:
                    return array.ToObject<IList<String>>().ToDictionary(k => k, v =>
                    {
                        return config.Categories[config.Categories.ContainsKey(v) ? v : "Default"];
                    });
                default:
                    break;
            }

            return null;
        }

        public override Boolean CanConvert(Type objectType) => objectType == typeof(IDictionary<String, String>);
    }
}