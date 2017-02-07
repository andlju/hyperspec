using System;
using System.Reflection.Emit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyperspec.Hal
{
    public class HalConverter : JsonConverter
    {
        private readonly JsonSerializer _objectSerializer;
        private readonly Func<string> _linkBaseFunc;

        public HalConverter(JsonSerializer objectSerializer, Func<string> linkBaseFunc)
        {
            _objectSerializer = objectSerializer;
            _linkBaseFunc = linkBaseFunc;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var halResource = (Representation)value;
            
            try
            {
                JObject obj = new JObject();
                foreach (var contentContext in halResource.GetContent())
                {
                    var content = contentContext.Content;
                    if (content == null)
                        continue; // Don't ever try to serialize a null object

                    var contentObj = JObject.FromObject(content, _objectSerializer);
                    foreach (JProperty prop in contentObj.Children())
                    {
                        if (contentContext.IncludeProperty(prop.Name, prop.Value))
                        {
                            obj[prop.Name] = prop.Value;
                        }
                    }
                }

                var embedded = halResource.GetEmbedded();
                if (embedded != null && embedded.Count > 0)
                {
                    obj.Add("_embedded", JObject.FromObject(embedded, serializer));
                }

                var linkBase = _linkBaseFunc();

                var links = halResource.GetLinks(linkBase);
                if (links != null && links.Count > 0)
                {
                    obj.Add("_links", JObject.FromObject(links, serializer));
                }
                var forms = halResource.GetForms(linkBase);
                if (forms != null && forms.Count > 0)
                {
                    obj.Add("_forms", JObject.FromObject(forms, serializer));
                }

                obj.WriteTo(writer);
            }
            catch (Exception ex)
            {
                // If there is an error during serialization, let's report it
                JObject errorObj = JObject.FromObject(
                    new {code = ex.Message.GetHashCode(), message=ex.ToString()});
                errorObj.WriteTo(writer);
            }
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