using System.Collections.Generic;

namespace Hyperspec
{
    public static class OwinExtensions
    {
        public static string GetLinkBase(this IDictionary<string, object> owinEnvironment)
        {
            var scheme = (string)owinEnvironment["owin.RequestScheme"];
            var headers = (IDictionary<string, string[]>)owinEnvironment["owin.RequestHeaders"];
            var host = headers["Host"][0];
            var pathBase = (string)owinEnvironment["owin.RequestPathBase"];

            return string.Format("{0}://{1}{2}", scheme, host, pathBase);
        }

    }
}