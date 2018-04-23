using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FriendsApi.Models;
using FriendsApi.Representations;
using Nancy;

namespace FriendsApi.Modules
{
    public class FriendsModule : NancyModule
    {
        public FriendsModule()
        {
            // Make sure to call with the HTTP header
            // Accept: application/hal+json
            Get[FriendsLinks.Friends] = _ =>
            {
                var friend1 = new Friend()
                {
                    FullName = "Anders Ljusberg",
                    Slug = "anderslj",
                    Blog = "http://coding-insomnia.com",
                    Workplace = new Company()
                    {
                        Name = "Aptitud",
                        Web = "https://aptitud.se"
                    }
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