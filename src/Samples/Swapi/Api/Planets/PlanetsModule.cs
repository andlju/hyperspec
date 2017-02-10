using Hyperspec.Nancy;
using Nancy;
using Nancy.Responses;
using Nancy.Responses.Negotiation;

namespace Swapi.Api.Planets
{
    public class PlanetsModule : NancyModule
    {
        public PlanetsModule(IPlanetsService planetsService)
        {
            Get[PlanetLinks.Planets] = _ =>
            {
                int page = Request.Query.page.Default(1);

                var planetsResponse = planetsService.GetPlanets(page);
                var repr = new PlanetsRepresentation(planetsResponse.Page, planetsResponse.Count, planetsResponse.HasMore);
                foreach (var planet in planetsResponse.Planets)
                {
                    repr.EmbedResource("planet", new PlanetRepresentation(planet));
                }
                return repr;
            };

            Get[PlanetLinks.Planet] = pars =>
            {
                string planetId = pars.planetId;
                var planet = planetsService.GetPlanet(planetId);
                var repr = new PlanetRepresentation(planet);
                return repr;
            };
        }   
    }
}