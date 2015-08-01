using Hyperspec;

namespace Swapi.Api.Planets
{
    public class PlanetsRepresentation : Representation
    {
        public PlanetsRepresentation(int page, int total, bool hasMore) : base(PlanetLinks.Planets)
        {
            Page = page;
            Total = total;
            HasMore = hasMore;
        }

        private bool HasMore { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }

        protected override void AddLinks(ILinkBuilder linkBuilder)
        {
            base.AddLinks(linkBuilder);
            if (HasMore) 
            { 
                linkBuilder.AddLink("next", PlanetLinks.Planets, "Next page", new { page = Page + 1 });
            }
        }
    }
}