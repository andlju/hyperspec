using System;
using System.Collections.Generic;
using Hyperspec.Hal;
using Nancy;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;

namespace Hyperspec.Nancy
{
    public class HalResourceResponseProcessor : IResponseProcessor
    {
        private readonly ISerializer _serializer;

        private static readonly IEnumerable<Tuple<string, MediaRange>> extensionMappings =
            new[] { new Tuple<string, MediaRange>("hal", MediaRange.FromString("application/hal+json")) };

        public HalResourceResponseProcessor(JsonSerializer objectSerializer)
        {
            _serializer = new HalNancySerializer(objectSerializer);
        }

        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            if (requestedMediaRange.Matches("application/hal+json"))
            {
                if (model is Representation)
                    return new ProcessorMatch()
                    {
                        ModelResult = MatchResult.ExactMatch,
                        RequestedContentTypeResult = MatchResult.ExactMatch
                    };
            }
            return new ProcessorMatch()
            {
                ModelResult = MatchResult.NoMatch,
                RequestedContentTypeResult = MatchResult.NoMatch
            };
        }

        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            var halModel = model as Representation;

            return new HalResponse(halModel, _serializer);
        }

        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get { return extensionMappings; }
        }
    }
}