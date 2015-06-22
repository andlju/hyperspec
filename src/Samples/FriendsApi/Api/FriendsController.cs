using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Http;
using FriendsApi.Models;
using FriendsApi.Representations;

namespace FriendsApi.Api
{
    public class FriendsController : ApiController
    {
        [Route(FriendsLinks.Friends)]
        public FriendsRepresentation Get()
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
        }

    }
}