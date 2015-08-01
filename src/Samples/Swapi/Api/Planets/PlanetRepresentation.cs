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
    }
}