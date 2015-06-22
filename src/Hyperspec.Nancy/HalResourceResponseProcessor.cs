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
        private readonly JsonSerializer _objectSerializer;

        private static readonly IEnumerable<Tuple<string, MediaRange>> extensionMappings =
            new[] { new Tuple<string, MediaRange>("hal", new MediaRange("application/hal+json")) };

        public HalResourceResponseProcessor(JsonSerializer objectSerializer)
        {
            _objectSerializer = objectSerializer;
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

            return new HalResponse(halModel, new HalNancySerializer(_objectSerializer, context));
        }

        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get { return extensionMappings; }
        }
    }
}