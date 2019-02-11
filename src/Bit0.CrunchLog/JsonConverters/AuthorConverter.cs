using Bit0.CrunchLog.Config;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Bit0.CrunchLog.JsonConverters
{
    public class AuthorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            var user = (Author)value;

            writer.WriteValue(user.Alias);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var config = ServiceProviderFactory.Current.GetService<CrunchSite>();
            
            var authorKey = (String)reader.Value;

            if (!String.IsNullOrWhiteSpace(authorKey)
                && config.Authors.ContainsKey(authorKey))
            {
                return config.Authors[authorKey];
            }
            else
            {
                var author = config.Authors.FirstOrDefault();
                return author.Value;
            }
        }

        public override Boolean CanConvert(Type objectType)
        {
            return objectType == typeof(Author);
        }
    }
}