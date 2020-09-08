using Bit0.CrunchLog.Config;
using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.JsonConverters
{
    public class ThemeConverter : JsonConverter
    {
        public override Boolean CanConvert(Type objectType) => objectType == typeof(Theme);

        public override void WriteJson(JsonWriter writer, Object value, JsonSerializer serializer)
        {
            writer.WriteValue(((Theme)value).OriginalName);
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, JsonSerializer serializer)
        {
            return new Theme((String)reader.Value);
        }
    }
}
