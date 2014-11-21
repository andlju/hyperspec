using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyperspec;
using Nancy;

namespace FriendsApi.Modules
{
    public class Friend
    {
        public string FullName { get; set; }
        public string Blog { get; set; }
        public string Slug { get; set; }
    }

    public static class FriendsLinks
    {
        public static Link Friends = new Link("/friends");
        public static Link Friend = new Link("/friend/{slug}");
    }

    public class FriendRepresentation : Representation<Friend>
    {
        public FriendRepresentation(Friend content, string resourceType) 
            : base(content, FriendsLinks.Friends, resourceType)
        {
        }


    }

    public class FriendsModule : NancyModule
    {
        public FriendsModule()
        {
            Get[FriendsLinks.Friends.GetPathTemplate()] = _ => "friends";
        }
    }
}