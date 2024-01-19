// PlanetTypes.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
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

        private static List<Registry> ColdTextures = new() { GameSpriteRegistries.cold, GameSpriteRegistries.cold1, GameSpriteRegistries.cold2, GameSpriteRegistries.cold3 };

        private static List<Registry> DryTextures = new() { GameSpriteRegistries.dry, GameSpriteRegistries.dry1, GameSpriteRegistries.dry2, GameSpriteRegistries.dry3 };

        private static List<Registry> StoneTextures = new() { GameSpriteRegistries.stone, GameSpriteRegistries.stone1, GameSpriteRegistries.stone2, GameSpriteRegistries.stone3 };

        private static List<Registry> GasTextures = new() { GameSpriteRegistries.gas, GameSpriteRegistries.gas1, GameSpriteRegistries.gas2, GameSpriteRegistries.gas3 };

        private static List<Registry> WarmTextures = new() { GameSpriteRegistries.warm, GameSpriteRegistries.warm1, GameSpriteRegistries.warm2, GameSpriteRegistries.warm3 };

        private static List<Registry> TerrTextures = new() { GameSpriteRegistries.terrestrial, GameSpriteRegistries.terrestrial1, GameSpriteRegistries.terrestrial2, GameSpriteRegistries.terrestrial3 };

        private static List<Registry> Tropical = new() { GameSpriteRegistries.tropical1, GameSpriteRegistries.tropical2, GameSpriteRegistries.tropical3, GameSpriteRegistries.tropical3 };

        private static Planet mPlanet;

        public class Cold
        {
            public Cold(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(ColdTextures), ColdScale); }

            public static implicit operator Planet(Cold c) { return mPlanet; }
        }

        public class Dry
        {
            public Dry(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(DryTextures), DryScale); }

            public static implicit operator Planet(Dry c) { return mPlanet; }
        }

        public class Gas
        {
            public Gas(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(GasTextures), GasScale); }

            public static implicit operator Planet(Gas c) { return mPlanet; }
        }

        public class Stone
        {
            public Stone(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(StoneTextures), StoneScale); }

            public static implicit operator Planet(Stone c) { return mPlanet; }
        }

        public class Tessatial
        {
            public Tessatial(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(TerrTextures), TerrScale); }

            public static implicit operator Planet(Tessatial c) { return mPlanet; }
        }

        public class Warm
        {
            public Warm(Vector2 orbitCenter, int orbitRadius) { mPlanet = new(orbitCenter, orbitRadius, ExtendetRandom.GetRandomElement(WarmTextures), WarmScale); }

            public static implicit operator Planet(Warm c) { return mPlanet; }
        }
    }
}
