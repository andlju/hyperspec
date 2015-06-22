using System.Web.Http;
using Hyperspec.WebApi;

namespace FriendsApi
{
    public static class WebApiConfiguration
    {
        public static HttpConfiguration Initialize()
        {
            var config = new HttpConfiguration();
            
            config.MapHttpAttributeRoutes();
            config.EnableHyperspec();

            return config;
        }
    }
}