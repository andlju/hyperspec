using System.Web.Http;

namespace Hyperspec.WebApi
{
    public static class HyperspecConfiguration
    {
        public static void EnableHyperspec(this HttpConfiguration config)
        {
            config.Formatters.Add(new HalJsonFormatter());
        }

    }
}