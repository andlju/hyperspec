using Hyperspec;

namespace FriendsApi.Representations
{
    /// <summary>
    /// A collection of friends
    /// </summary>
    public class FriendsRepresentation : Representation
    {
        public FriendsRepresentation() : base(FriendsLinks.Friends, "frapi:friends")
        {
            
        }
    }
}