using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FriendsApi.Models;
using Hyperspec;
using Nancy;

namespace FriendsApi.Modules
{
    public static class FriendsLinks
    {
        public static Link Friends = new Link("/friends");
        public static Link Friend = new Link("/friends/{slug}");

        public static Link Image = new Link("/image/{slug}");
    }

    public class FriendRepresentation : Representation<Friend>
    {
        public FriendRepresentation(Friend content) 
            : base(content, FriendsLinks.Friend, "friend")
        {
        }

        protected override void AddLinks(IResourceLinkBuilder linkBuilder)
        {
            linkBuilder.AddLink("image", FriendsLinks.Image, prompt: "Image");
            linkBuilder.AddLink("blog", new Link(Content.Blog), prompt: "Blog");
        }
    }

    public class FriendsRepresentation : Representation
    {
        public FriendsRepresentation() : base(FriendsLinks.Friends, "friends")
        {
            
        }

    }

    
    public class FriendsModule : NancyModule
    {
        public FriendsModule()
        {
            Get[FriendsLinks.Friends.GetPathTemplate()] = _ =>
            {
                var friend1 = new Friend()
                {
                    FullName = "Anders Ljusberg",
                    Slug = "anderslj",
                    Blog = "http://coding-insomnia.com"
                };
                var friend2 = new Friend()
                {
                    FullName = "Glenn Block",
                    Slug = "gblock",
                    Blog = "http://codebetter.com/glennblock/"
                };

                var friends = new FriendsRepresentation();
                friends.EmbedResource("friend", new FriendRepresentation(friend1));
                friends.EmbedResource("friend", new FriendRepresentation(friend2));

                return friends;
            };
        }
    }
}