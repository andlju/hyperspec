using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyperspec.Hal
{
    public class HalLinksConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var links = ((IEnumerable<IResourceLink>)value).ToArray();
            if (links.Length == 1)
            {
                GetLinkObject(links[0], serializer).WriteTo(writer);
                return;
            }
            JArray array = new JArray();
            foreach (var link in links)
            {
                var linkObj = GetLinkObject(link, serializer);
                array.Add(linkObj);
            }
            array.WriteTo(writer);
        }

        public JObject GetLinkObject(IResourceLink link, JsonSerializer serializer)
        {
            var linkObj = JObject.FromObject(link, serializer);
            var templated = linkObj["href"].Value<string>().Contains("{");
            if (templated)
            {
                linkObj.Add("templated", link.Href.Contains("{"));
            }
            return linkObj;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (typeof(IEnumerable<IResourceLink>).IsAssignableFrom(objectType));
        }
    }

}