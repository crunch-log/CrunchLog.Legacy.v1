using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bit0.CrunchLog.JsonConverters
{
    public class TagsConverters : JsonConverter
    {
        public override Boolean CanConvert(Type objectType) => objectType == typeof(IDictionary<String, CategoryInfo>);

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var keys = ((IDictionary<String, CategoryInfo>)value).Keys.ToList();
            serializer.Serialize(writer, keys);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var tags = token.ToObject<IEnumerable<String>>()
                .ToDictionary(k => k, v =>
                {
                    return new CategoryInfo
                    {
                        Title = v,
                        Permalink = String.Format(StaticKeys.TagPathFormat, v)
                    };
                });

            return tags;
        }
    }
}
