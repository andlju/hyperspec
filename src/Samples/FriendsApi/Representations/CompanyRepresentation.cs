using FriendsApi.Models;
using Hyperspec;

namespace FriendsApi.Representations
{
    /// <summary>
    /// Representation for a Company
    /// </summary>
    public class CompanyRepresentation : Representation<Company>
    {
        public CompanyRepresentation(Company content) : base(content, null, "frapi:company")
        {
        }

        protected override void AddLinks(ILinkBuilder linkBuilder)
        {
            linkBuilder.AddLink("web", Content.Web, prompt: "Web");
        }
    }
}