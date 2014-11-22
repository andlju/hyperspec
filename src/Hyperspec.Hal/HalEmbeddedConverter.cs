using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Hyperspec.Hal
{
    public class HalEmbeddedConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var embedded = ((IEnumerable<Representation>)value).ToArray();
            writer.WriteStartArray();
            foreach (var item in embedded)
            {
                serializer.Serialize(writer, item);
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (typeof(IEnumerable<Representation>).IsAssignableFrom(objectType));
        }
    }
}