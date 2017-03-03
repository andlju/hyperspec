using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using Hyperspec;
using Swapi.Model;

namespace Swapi.Api.Planets
{

    public class PlanetRepresentation : Representation<Planet>
    {
        public PlanetRepresentation(Planet content) : base(content, PlanetLinks.Planet, "swapi:planet")
        {

        }

        protected override void AddLinks(ILinkBuilder linkBuilder)
        {
            linkBuilder.AddLink<PlanetSearchRequest>("similar", PlanetLinks.Planets, "Find similar planets",null, new List<TemplateParameterInfo>()
            {
                new TemplateParameterInfo()
                {
                    Name = "Climate",
                    ForceTemplated = true
                }
            });
        }

        protected override void AddForms(IFormBuilder formBuilder)
        {
            formBuilder.AddForm<InvestigatePlanetCommand>("command", PlanetLinks.InvestigateCommand, "Investigate Planet", "POST", null);
            formBuilder.AddForm<DestroyPlanetCommand>("command", PlanetLinks.DestroyCommand, "Destroy Planet", "POST", null);
        }
    }
}