namespace Swapi.Api.Planets
{
    public static class PlanetLinks
    {
        public const string Planets = "/api/planets";

        public const string Planet = Planets + "/{planetId}";

        public const string InvestigateCommand = Planet + "/investigate";
        public const string DestroyCommand = Planet + "/destroy";
    }
}