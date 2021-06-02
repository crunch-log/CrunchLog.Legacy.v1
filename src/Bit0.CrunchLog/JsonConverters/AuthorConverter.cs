using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bit0.CrunchLog.JsonConverters
{
    public class AuthorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var users = ( (IEnumerable<Author>)value ).Select(a => a.Alias);
            writer.WriteValue(users);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            switch(reader.TokenType)
            {
                case JsonToken.StartArray:
                    var authors = JToken.Load(reader).ToObject<IEnumerable<String>>();
                    return authors.Select(a => GetAuthor(a));
                case JsonToken.String:
                    return new List<Author> { GetAuthor((String)reader.Value) };
            }


            return new List<Author> { GetAuthor() };
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<Author>);
        }

        private Author GetAuthor(String authorKey = null)
        {
            var config = ServiceProviderFactory.Current.GetService<CrunchConfig>();
            var defaultAuthor = config.Authors.FirstOrDefault().Value;

            if(!String.IsNullOrWhiteSpace(authorKey)
                        && config.Authors.ContainsKey(authorKey))
            {
                return config.Authors[authorKey];
            }

            return defaultAuthor;
        }
    }
}