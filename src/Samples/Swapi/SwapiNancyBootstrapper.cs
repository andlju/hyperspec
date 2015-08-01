using Hyperspec.Nancy;
using Nancy;
using Nancy.TinyIoc;

namespace Swapi
{
    public class SwapiNancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<ISerializer, HalNancySerializer>();
        }
         
    }
}