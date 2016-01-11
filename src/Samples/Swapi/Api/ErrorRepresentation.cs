using Hyperspec;

namespace Swapi.Api
{
    public class ErrorRepresentation : Representation
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}