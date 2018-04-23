using System.Collections.Generic;
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
            if (content.Workplace != null)
            {
                EmbedResource("workplace", new CompanyRepresentation(content.Workplace), true);
            }
        }

        // The self link is added automatically
        protected override void AddLinks(ILinkBuilder linkBuilder)
        {
            linkBuilder.AddLink("image", FriendsLinks.Image, prompt: "Image");
            linkBuilder.AddLink("blog", Content.Blog, prompt: "Blog");
        }

        protected override IEnumerable<string> PropertiesToIgnore()
        {
            yield return "Workplace";
        }
    }
}