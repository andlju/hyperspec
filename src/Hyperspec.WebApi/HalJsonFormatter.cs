using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using Hyperspec.Hal;
using Microsoft.Owin;

namespace Hyperspec.WebApi
{
    public class HalJsonFormatter : JsonMediaTypeFormatter
    {
        private readonly string _basePath;

        private const string OwinEnvironmentKey = "MS_OwinEnvironment";
        private const string OwinContextKey = "MS_OwinContext";

        public HalJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
            SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            _basePath = "";
        }

        private HalJsonFormatter(string basePath) : this()
        {
            _basePath = basePath;
        }

        public override Newtonsoft.Json.JsonSerializer CreateJsonSerializer()
        {
            var objectSerializer = base.CreateJsonSerializer();
            return new HalJsonSerializer(objectSerializer, () => _basePath);
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request,
            MediaTypeHeaderValue mediaType)
        {
            IDictionary<string, object> owinEnv = null;
            object obj;
            request.Properties.TryGetValue(OwinContextKey, out obj);
            
            var owinContext = obj as IOwinContext;
            if (owinContext != null)
            {
                owinEnv = owinContext.Environment;
            }
            
            if (owinEnv == null)
            {
                request.Properties.TryGetValue(OwinEnvironmentKey, out obj);
                owinEnv = obj as IDictionary<string, object>;
            }
            
            if (owinEnv == null)
            {
                // No Owin environment found. Perhaps not running under Owin? 
                // We don't care, instead let's consider the linkBase to be empty
                return this; 
            }

            // Found an Owin environment, let's use it to find the link base.
            var linkBase = owinEnv.GetLinkBase();
            
            return new HalJsonFormatter(linkBase);
        }

        public override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type) &&
                   (typeof (Representation).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) ||
                    typeof (Representation).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()));
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(
            Type type, 
            object value, 
            Stream writeStream,
            HttpContent content, 
            System.Net.TransportContext transportContext)
        {
            if (typeof (Representation).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) &&
                type != typeof (Representation))
            {
                return base.WriteToStreamAsync(typeof (Representation), value, writeStream, content, transportContext);
            }

            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

    }
}