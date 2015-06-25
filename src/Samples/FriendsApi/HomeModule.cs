using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyperspec.Nancy;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace FriendsApi
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello Nancy";
        }
    }
}   