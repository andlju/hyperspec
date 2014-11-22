using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hyperspec.Hal
{
    public class HalJsonSerializer : JsonSerializer
    {
        public HalJsonSerializer(JsonSerializer objectSerializer)
        {
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.NullValueHandling = NullValueHandling.Ignore;
            this.Formatting = Formatting.Indented;

            this.Converters.Add(new HalConverter(objectSerializer));
            this.Converters.Add(new HalLinksConverter());
            this.Converters.Add(new HalFormConverter());
            this.Converters.Add(new HalEmbeddedConverter());
        }
    }
}