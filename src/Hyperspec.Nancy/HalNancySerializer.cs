using System.Collections.Generic;
using System.IO;
using Hyperspec.Hal;
using Nancy;
using Nancy.IO;
using Nancy.Json;
using Newtonsoft.Json;

namespace Hyperspec.Nancy
{

    public class HalNancySerializer : ISerializer
    {
        private readonly JsonSerializer _serializer;

        public HalNancySerializer(JsonSerializer objectSerializer, NancyContext context)
        {
            _serializer = new HalJsonSerializer(objectSerializer, () => context.Request.Url.SiteBase);
        }

        public bool CanSerialize(string contentType)
        {
            return contentType == "application/hal+json";
        }

        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            using (var writer = new JsonTextWriter(new StreamWriter(new UnclosableStreamWrapper(outputStream))))
            {
                _serializer.Serialize(writer, model);
            }
        }

        public IEnumerable<string> Extensions
        {
            get { yield return "hal"; }
        }
    }
}