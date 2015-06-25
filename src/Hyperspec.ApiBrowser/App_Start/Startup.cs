using Nancy.Routing;
using Owin;

namespace Hyperspec.ApiBrowser
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(opt =>
            {
                opt.PerformPassThrough = ctx => ctx.ResolvedRoute is NotFoundRoute;
            });
        }
    }
}