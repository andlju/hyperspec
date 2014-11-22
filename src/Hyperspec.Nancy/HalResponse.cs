using System;
using System.IO;
using Nancy;

namespace Hyperspec.Nancy
{
    public class HalResponse<TModel> : Response
    {
        public HalResponse(TModel model, ISerializer serializer)
        {
            if (serializer == null)
            {
                throw new InvalidOperationException("JSON Serializer not set");
            }

            this.Contents = GetJsonContents(model, serializer);
            this.ContentType = "application/hal+json";
            this.StatusCode = HttpStatusCode.OK;
        }

        private static Action<Stream> GetJsonContents(TModel model, ISerializer serializer)
        {
            return stream => serializer.Serialize("application/hal+json", model, stream);
        }
    }

    public class HalResponse : HalResponse<object>
    {
        public HalResponse(object model, ISerializer serializer)
            : base(model, serializer)
        {
        }
    }
}