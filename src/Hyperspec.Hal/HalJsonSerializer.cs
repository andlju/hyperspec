using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hyperspec.Hal
{
    public class HalJsonSerializer : JsonSerializer
    {
        private readonly Func<string> _linkBaseFunc;

        public HalJsonSerializer(JsonSerializer objectSerializer, Func<string> linkBaseFunc)
        {
            _linkBaseFunc = linkBaseFunc;
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.NullValueHandling = NullValueHandling.Ignore;
            this.Formatting = Formatting.Indented;

            this.Converters.Add(new HalConverter(objectSerializer, _linkBaseFunc));
            this.Converters.Add(new HalLinksConverter());
            this.Converters.Add(new HalFormConverter());
            this.Converters.Add(new HalEmbeddedConverter());
        }
    }
}