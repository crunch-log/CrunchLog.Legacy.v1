using System;
using Bit0.CrunchLog.Config;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Convertors
{
    public class AuthorConvertor : JsonConverter
    {
        private readonly CrunchConfig _config;

        public AuthorConvertor()
        {
            var provider = ServiceProviderFactory.ServiceProvider;
            _config = provider.GetService<CrunchConfig>();
        }

        public override Boolean CanConvert(Type objectType)
        {
            return (objectType == typeof(String));
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            var author = reader.Value.ToString();

            return !_config.Authors.ContainsKey(author) 
                ? new Author{ Name = author } 
                : _config.Authors[author];
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Left as an exercise to the reader :)
            throw new NotImplementedException();
        }
    }
}
