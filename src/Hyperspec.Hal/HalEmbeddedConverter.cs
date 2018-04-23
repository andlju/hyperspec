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
            var embedded = (Embedded)value;
            if (embedded.Single)
            {
                var singleItem = embedded.GetEmbedded().FirstOrDefault();
                if (singleItem != null)
                {
                    serializer.Serialize(writer, singleItem);
                }
            }
            else
            {
                writer.WriteStartArray();
                var items = embedded.GetEmbedded();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        serializer.Serialize(writer, item);
                    }
                }
                writer.WriteEndArray();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (typeof(Embedded).IsAssignableFrom(objectType));
        }
    }
}