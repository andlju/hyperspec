using System.Collections.Generic;
using System.IO;
using Hyperspec.Hal;
using Nancy;
using Nancy.IO;
using Newtonsoft.Json;

namespace Hyperspec.Nancy
{
    public class HalNancySerializer : ISerializer
    {
        private JsonSerializer _serializer;

        public HalNancySerializer(JsonSerializer objectSerializer)
        {
            _serializer = new HalJsonSerializer(objectSerializer);
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