// PlanetTypes.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Utilitys;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    internal static class PlanetTypes
    {
        private static float ColdScale { get { return 5 * ExtendetRandom.Random.Next(2, 3); } }
        private static float DryScale { get { return 5 * ExtendetRandom.Random.Next(3, 4); } }
        private static float GasScale { get { return 5 * ExtendetRandom.Random.Next(5, 7); } }
        private static float StoneScale { get { return 5 * ExtendetRandom.Random.Next(2, 3); } }
        private static float TerrScale { get { return 5 * ExtendetRandom.Random.Next(3, 4); } }
        private static float WarmScale { get { return 5 * ExtendetRandom.Random.Next(3, 4); } }

        private static List<Registry> ColdTextures = new() { TextureRegistries.cold1, TextureRegistries.cold2, TextureRegistries.cold3, TextureRegistries.cold4 };
        private static List<Registry> DryTextures = new() { TextureRegistries.dry1, TextureRegistries.dry2, TextureRegistries.dry3, TextureRegistries.dry4, TextureRegistries.dry5, TextureRegistries.dry6 };
        private static List<Registry> StoneTextures = new() { TextureRegistries.stone1, TextureRegistries.stone2, TextureRegistries.stone3, TextureRegistries.stone4, TextureRegistries.stone5, TextureRegistries.stone6 };
        private static List<Registry> GasTextures = new() { TextureRegistries.gas1, TextureRegistries.gas2, TextureRegistries.gas3, TextureRegistries.gas4 };
        private static List<Registry> WarmTextures = new() { TextureRegistries.warm1, TextureRegistries.warm2, TextureRegistries.warm3, TextureRegistries.warm4 };
        private static List<Registry> TerrTextures = new() { TextureRegistries.terrestrial1, TextureRegistries.terrestrial2, TextureRegistries.terrestrial3, TextureRegistries.terrestrial4,
            TextureRegistries.terrestrial5, TextureRegistries.terrestrial6, TextureRegistries.terrestrial7, TextureRegistries.terrestrial8 };

        public class Cold
        {
            private Planet mPlanet;

            public Cold(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(ColdTextures), ColdScale); }

            public static implicit operator Planet(Cold c) { return c.mPlanet; }
        }

        public class Dry
        {
            private Planet mPlanet;

            public Dry(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(DryTextures), DryScale); }

            public static implicit operator Planet(Dry c) { return c.mPlanet; }
        }

        public class Gas
        {
            private Planet mPlanet;

            public Gas(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(GasTextures), GasScale); }

            public static implicit operator Planet(Gas c) { return c.mPlanet; }
        }

        public class Stone
        {
            private Planet mPlanet;

            public Stone(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(StoneTextures), StoneScale); }

            public static implicit operator Planet(Stone c) { return c.mPlanet; }
        }

        public class Tessatial
        {
            private Planet mPlanet;

            public Tessatial(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(TerrTextures), TerrScale); }

            public static implicit operator Planet(Tessatial c) { return c.mPlanet; }
        }

        public class Warm
        {
            private Planet mPlanet;

            public Warm(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(WarmTextures), WarmScale); }

            public static implicit operator Planet(Warm c) { return c.mPlanet; }
        }
    }
}
