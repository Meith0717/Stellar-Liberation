using CelestialOdyssey.Core.GameEngine.Utility;
using CelestialOdyssey.Core.GameObjects.Types;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.Astronomical_Body
{
    public class PlanetTypes
    {
        // Cold
        private readonly static PlanetType c1 = new(.0505f, "cold1");
        private readonly static PlanetType c2 = new(.052f, "cold2");
        private readonly static PlanetType c3 = new(.05f, "cold3");
        private readonly static PlanetType c4 = new(.051f, "cold4");
        // Dry
        private readonly static PlanetType d1 = new(.2f, "dry1");
        private readonly static PlanetType d2 = new(.21f, "dry2");
        private readonly static PlanetType d3 = new(.205f, "dry3");
        private readonly static PlanetType d4 = new(.22f, "dry4");
        private readonly static PlanetType d5 = new(.19f, "dry5");
        private readonly static PlanetType d6 = new(.21f, "dry6");
        // Gas
        private readonly static PlanetType g1 = new(.515f, "gas1");
        private readonly static PlanetType g2 = new(.5f, "gas2");
        private readonly static PlanetType g3 = new(.52f, "gas3");
        private readonly static PlanetType g4 = new(.51f, "gas4");
        // Stone 
        private readonly static PlanetType s1 = new(.201f, "stone1");
        private readonly static PlanetType s2 = new(.21f, "stone2");
        private readonly static PlanetType s3 = new(.205f, "stone3");
        private readonly static PlanetType s4 = new(.2f, "stone4");
        private readonly static PlanetType s5 = new(.19f, "stone5");
        private readonly static PlanetType s6 = new(.215f, "stone6");
        // Terrestrial
        private readonly static PlanetType t1 = new(.25f, "terrestrial1");
        private readonly static PlanetType t2 = new(.2f, "terrestrial2");
        private readonly static PlanetType t3 = new(.305f, "terrestrial3");
        private readonly static PlanetType t4 = new(.27f, "terrestrial4");
        private readonly static PlanetType t5 = new(.29f, "terrestrial5");
        private readonly static PlanetType t6 = new(.3f, "terrestrial6");
        private readonly static PlanetType t7 = new(.305f, "terrestrial7");
        private readonly static PlanetType t8 = new(.31f, "terrestrial8");
        // Warm
        private readonly static PlanetType w1 = new(.2f, "warm1");
        private readonly static PlanetType w2 = new(.2f, "warm2");
        private readonly static PlanetType w3 = new(.2f, "warm3");
        private readonly static PlanetType w4 = new(.2f, "warm4");

        // Type Lists
        private readonly static List<PlanetType> Orbit_1 = new()
        { 
            // Warm
            w1, w2, w3, w4,
            // Stone
            s1 , s2 , s3 , s4 , s5 , s6,
        };

        private readonly static List<PlanetType> Orbit_2 = new()
        {
            // Stone
            s1 , s2 , s3 , s4 , s5 , s6,
            // Dry
            d1, d2, d3, d4, d5, d6
        };

        private readonly static List<PlanetType> Orbit_3 = new()
        {
            // Dry
            d1, d2, d3, d4, d5, d6,
            // Terrest
            t1 , t2 , t3 , t4 , t5 , t6, t7, t8
        };

        private readonly static List<PlanetType> Orbit_4 = new()
        {
            // Stone
            s1 , s2 , s3 , s4 , s5 , s6,
            // Gas
            g1, g2, g3, g4
        };

        private readonly static List<PlanetType> Orbit_5 = new()
        {
            // Gas
            g1, g2, g3, g4,
            // Cold
            c1, c2, c3, c4,
        };

        public static PlanetType GetOrbit1
        {
            get { return Utility.GetRandomElement(Orbit_1); }
        }
        public static PlanetType GetOrbit2
        {
            get { return Utility.GetRandomElement(Orbit_2); }
        }
        public static PlanetType GetOrbit3
        {
            get { return Utility.GetRandomElement(Orbit_3); }
        }
        public static PlanetType GetOrbit4
        {
            get { return Utility.GetRandomElement(Orbit_4); }
        }
        public static PlanetType GetOrbit5
        {
            get { return Utility.GetRandomElement(Orbit_5); }
        }
        public static PlanetType GetOrbit6
        {
            get { return Utility.GetRandomElement(Orbit_5); }
        }
    }
}
