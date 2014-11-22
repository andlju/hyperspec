using System.Linq;
using Nancy;

namespace Hyperspec.Nancy
{
    public static class HalResponseExtensions
    {
        public static HalResponse<TModel> AsHal<TModel>(this IResponseFormatter responseFormatter, TModel model)
        {
            return new HalResponse<TModel>(model, responseFormatter.Serializers.FirstOrDefault(s => s.CanSerialize("application/hal+json")));
        }
    }
}