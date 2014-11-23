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
        public static TemplatedLink Friends = new TemplatedLink("/friends");
        public static TemplatedLink Friend = new TemplatedLink("/friends/{slug}");

        public static TemplatedLink Image = new TemplatedLink("/image/{slug}");
    }

    /// <summary>
    /// Representation for a single friend. 
    /// </summary>
    public class FriendRepresentation : Representation<Friend>
    {
        public FriendRepresentation(Friend content) 
            : base(content, FriendsLinks.Friend, "frapi:friend")
        {
        }

        // The self link is added automatically
        protected override void AddLinks(ILinkBuilder linkBuilder)
        {
            linkBuilder.AddLink("image", FriendsLinks.Image, prompt: "Image");
            linkBuilder.AddLink("blog", Content.Blog, prompt: "Blog");
        }
    }

    /// <summary>
    /// A collection of friends
    /// </summary>
    public class FriendsRepresentation : Representation
    {
        public FriendsRepresentation() : base(FriendsLinks.Friends, "frapi:friends")
        {
            
        }
    }

    
    public class FriendsModule : NancyModule
    {
        public FriendsModule()
        {
            // Make sure to call with the HTTP header
            // Accept: application/hal+json
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

                // Create the represenation for a list of friends
                var friends = new FriendsRepresentation();

                // Create each friend resource and embed them in the list
                friends.EmbedResource("friend", new FriendRepresentation(friend1));
                friends.EmbedResource("friend", new FriendRepresentation(friend2));

                return friends;
            };
        }
    }
}