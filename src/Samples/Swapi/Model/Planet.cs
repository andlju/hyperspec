using System.Collections.Generic;

namespace Swapi.Model
{
    public class Planet
    {
        public string PlanetId { get; set; }

        public string Name { get; set; }
        public int? RotationPeriod { get; set; }
        public int? OrbitalPeriod { get; set; }
        public int? Diameter { get; set; }
        public string Climate { get; set; }
        public string Terrain { get; set; }
        public string Gravity { get; set; }
        public int? SurfaceWater { get; set; }
        public int? Population { get; set; }

        public IEnumerable<string> Residents { get; set; }
    }
}