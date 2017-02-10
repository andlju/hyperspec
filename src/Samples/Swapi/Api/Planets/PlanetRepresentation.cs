using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using Hyperspec;
using Swapi.Model;

namespace Swapi.Api.Planets
{
    public class PlanetQueryRequest
    {
        public string Climate { get; set; }
        public string Terrain { get; set; }
        public string Gravity { get; set; }
    }

    public class PlanetRepresentation : Representation<Planet>
    {
        public PlanetRepresentation(Planet content) : base(content, PlanetLinks.Planet, "swapi:planet")
        {

        }

        protected override void AddLinks(ILinkBuilder linkBuilder)
        {
            base.AddLinks(linkBuilder);
            linkBuilder.AddLink<PlanetQueryRequest>("similar", PlanetLinks.Planets, "Find similar planets",null, new List<TemplateParameterInfo>()
            {
                new TemplateParameterInfo()
                {
                    Name = "Climate",
                    ForceTemplated = true
                }
            });
        }
    }
}