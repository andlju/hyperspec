using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace FriendsApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/nancy", a => a.UseNancy());
            app.Map("/api", a => a.UseWebApi(WebApiConfiguration.Initialize()));
        }
    }
}