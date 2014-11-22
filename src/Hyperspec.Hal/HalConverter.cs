using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyperspec.Hal
{
    public class HalConverter : JsonConverter
    {
        private readonly JsonSerializer _objectSerializer;

        public HalConverter(JsonSerializer objectSerializer)
        {
            _objectSerializer = objectSerializer;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var halResource = (Representation)value;
            
            JObject obj = new JObject();

            foreach (var content in halResource.GetContent())
            {
                var contentObj = JObject.FromObject(content, _objectSerializer);
                foreach (JProperty prop in contentObj.Children())
                {
                    obj[prop.Name] = prop.Value;
                }
            }

            var embedded = halResource.GetEmbedded();
            if (embedded != null && embedded.Count > 0)
            {
                obj.Add("_embedded", JObject.FromObject(embedded, serializer));
            }

            var links = halResource.GetLinks();
            if (links != null && links.Count > 0)
            {
                obj.Add("_links", JObject.FromObject(links, serializer));
            }
            var forms = halResource.GetForms();
            if (forms != null && forms.Count > 0)
            {
                obj.Add("_forms", JObject.FromObject(forms, serializer));
            }

            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            if ((typeof(Representation)).IsAssignableFrom(objectType))
                return true;
            return false;
        }
    }
}