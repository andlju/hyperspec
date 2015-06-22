using FriendsApi.Models;
using Hyperspec;

namespace FriendsApi.Representations
{
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
}