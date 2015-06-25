using System.ComponentModel;
using Hyperspec.Nancy;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace FriendsApi
{
    public class FriendsApiBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<ISerializer, HalNancySerializer>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                            .WithHeader("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE")
                            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });
        }

    }
}