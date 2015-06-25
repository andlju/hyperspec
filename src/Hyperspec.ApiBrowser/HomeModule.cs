using Nancy;
using Nancy.Responses;

namespace Hyperspec.ApiBrowser
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => new RedirectResponse("/apibrowser/#?apiUrl=http:%2F%2Flocalhost:50248%2Fnancy%2Ffriends");
        } 
    }
}