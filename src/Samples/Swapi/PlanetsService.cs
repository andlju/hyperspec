using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swapi.Model;

namespace Swapi
{
    public class PlanetsResponse
    {
        public int Count { get; set; }
        public int Page { get; set; }
        public bool HasMore { get; set; }
        public IEnumerable<Planet> Planets { get; set; }
    }

    public class PlanetsService : IPlanetsService
    {
        public const string SwapiBaseUrl = "http://swapi.co/api";

        public PlanetsResponse GetPlanets(int page = 1)
        {
            using(var client = new HttpClient())
            using(var stream = client.GetStreamAsync(SwapiBaseUrl + "/planets/?page=" + page).Result)
            using(var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var planetsJson = JObject.Load(reader);

                var count = planetsJson["count"].Value<int>();
                var planets = planetsJson["results"].Select(ParsePlanet);
                var response = new PlanetsResponse()
                {
                    Count = count,
                    Page = page,
                    HasMore = planetsJson["next"] != null,

                    Planets = planets.ToArray()
                };
                return response;
            };
        }

        public Planet GetPlanet(string planetId)
        {
            using(var client = new HttpClient())
            using(var stream = client.GetStreamAsync(SwapiBaseUrl + "/planets/" + planetId +"/").Result)
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var planetsJson = JObject.Load(reader);
                return ParsePlanet(planetsJson);
            }
        }

        private static Planet ParsePlanet(JToken p)
        {
            return new Planet()
            {
                PlanetId = ExtractId(p["url"]),
                Name = p["name"].Value<string>(),
                Diameter = GetInteger(p["diameter"]),
                Gravity = p["gravity"].Value<string>(),
                Climate = p["climate"].Value<string>(),
                OrbitalPeriod = GetInteger(p["orbital_period"]),
                Population = GetInteger(p["population"]),
                RotationPeriod = GetInteger(p["rotation_period"]),
                SurfaceWater = GetInteger(p["surface_water"]),
                Terrain = p["terrain"].Value<string>(),
                Residents = p["residents"].Select(r => ExtractId(r))
            };
        }

        private static string ExtractId(JToken token)
        {
            return token.Value<string>().Split('/').Reverse().Skip(1).First();
        }

        private static int? GetInteger(JToken token)
        {
            int val;
            if (int.TryParse(token.Value<string>(), out val))
                return val;
            return null;
        }
    }
}