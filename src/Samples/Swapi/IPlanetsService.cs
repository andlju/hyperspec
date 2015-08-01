using Swapi.Model;

namespace Swapi
{
    public interface IPlanetsService
    {
        PlanetsResponse GetPlanets(int page = 1);
        Planet GetPlanet(string planetId);
    }
}