using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyperspec.Hal
{
    public class HalFormConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var forms = ((IEnumerable<IForm>)value).ToArray();
            if (forms.Length == 1)
            {
                serializer.Serialize(writer, forms[0]);
            }
            else
            {
                JArray array = new JArray();
                foreach (var form in forms)
                {
                    var formObj = JObject.FromObject(form, serializer);
                    array.Add(formObj);
                }
                array.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (typeof(IEnumerable<IForm>).IsAssignableFrom(objectType));
        }
    }
}