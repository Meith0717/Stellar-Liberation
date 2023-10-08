using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types
{
    internal static class PlanetTypes
    {
        private static float ColdScale { get { return 50 * Utility.Random.Next(8, 11); } }
        private static float DryScale { get { return 50 * Utility.Random.Next(10, 15); } }
        private static float GasScale { get { return 50 * Utility.Random.Next(20, 25); } }
        private static float StoneScale { get { return 50 * Utility.Random.Next(8, 11); } }
        private static float TerrScale { get { return 50 * Utility.Random.Next(10, 15); } }
        private static float WarmScale { get { return 50 * Utility.Random.Next(10, 15); } }

        private static List<Registry> ColdTextures = new() { ContentRegistry.cold1, ContentRegistry.cold2, ContentRegistry.cold3, ContentRegistry.cold4 };
        private static List<Registry> DryTextures = new() { ContentRegistry.dry1, ContentRegistry.dry2, ContentRegistry.dry3, ContentRegistry.dry4, ContentRegistry.dry5, ContentRegistry.dry6 };
        private static List<Registry> StoneTextures = new() { ContentRegistry.stone1, ContentRegistry.stone2, ContentRegistry.stone3, ContentRegistry.stone4, ContentRegistry.stone5, ContentRegistry.stone6 };
        private static List<Registry> GasTextures = new() { ContentRegistry.gas1, ContentRegistry.gas2, ContentRegistry.gas3, ContentRegistry.gas4 };
        private static List<Registry> WarmTextures = new() { ContentRegistry.warm1, ContentRegistry.warm2, ContentRegistry.warm3, ContentRegistry.warm4};
        private static List<Registry> TerrTextures = new() { ContentRegistry.terrestrial1, ContentRegistry.terrestrial2, ContentRegistry.terrestrial3, ContentRegistry.terrestrial4, 
            ContentRegistry.terrestrial5, ContentRegistry.terrestrial6, ContentRegistry.terrestrial7, ContentRegistry.terrestrial8 };

        public class Cold
        {
            private Planet mPlanet;

            public Cold(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, Utility.GetRandomElement(ColdTextures), ColdScale); }

            public static implicit operator Planet(Cold c) { return c.mPlanet; }
        }

        public class Dry
        {
            private Planet mPlanet;

            public Dry(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, Utility.GetRandomElement(DryTextures), DryScale); }

            public static implicit operator Planet(Dry c) { return c.mPlanet; }
        }

        public class Gas
        {
            private Planet mPlanet;

            public Gas(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, Utility.GetRandomElement(GasTextures), GasScale); }

            public static implicit operator Planet(Gas c) { return c.mPlanet; }
        }

        public class Stone
        {
            private Planet mPlanet;

            public Stone(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, Utility.GetRandomElement(StoneTextures), StoneScale); }

            public static implicit operator Planet(Stone c) { return c.mPlanet; }
        }

        public class Tessatial
        {
            private Planet mPlanet;

            public Tessatial(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, Utility.GetRandomElement(TerrTextures), TerrScale); }

            public static implicit operator Planet(Tessatial c) { return c.mPlanet; }
        }

        public class Warm
        {
            private Planet mPlanet;

            public Warm(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, Utility.GetRandomElement(WarmTextures), WarmScale); }

            public static implicit operator Planet(Warm c) { return c.mPlanet; }
        }
    }
}
