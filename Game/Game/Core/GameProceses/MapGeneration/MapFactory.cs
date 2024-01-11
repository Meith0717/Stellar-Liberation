// MapFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration
{
    public static class MapFactory
    {
        public readonly static int mSectorCountWidth = 70;
        public readonly static int mSectorCountHeight = 70;
        public readonly static int MapScale = 100;
        public readonly static int ViewScale = 1000000;

        public static void Generate(out HashSet<PlanetSystem> planetSystems)
        {
            planetSystems = new();
            var seededRandom = new Random(6464733);

            var noiseMap = BinaryMapGenerator.Generate(mSectorCountWidth, mSectorCountHeight, seededRandom);
            var positions = BinaryMapGenerator.GetVector2sFormBinaryMap(noiseMap);
            BinaryMapGenerator.ScaleVector2s(ref positions, MapScale);
            BinaryMapGenerator.ShiftVector2s(ref positions, 35, seededRandom);

            foreach( var position in positions )
            {
                var planetSystem = new PlanetSystem(position, Danger.None);
                planetSystem.SetObjects(StarGenerator.Generat(seededRandom));
                planetSystems.Add(planetSystem);
            }
        }

        public static List<Asteroid> GetAsteroidsRing(int numAsteroids, Vector2 center, float radiusMin, float radiusMax)
        {
            List<Asteroid> asteroids = new();
            Random random = ExtendetRandom.Random;

            for (int i = 0; i < numAsteroids; i++)
            {
                float angle = (float)random.NextDouble() * 2f * MathF.PI;
                float radius = (float)(random.NextDouble() * (radiusMax - radiusMin) + radiusMin);

                float x = center.X + radius * MathF.Cos(angle);
                float y = center.Y + radius * MathF.Sin(angle);

                asteroids.Add(new(new(x, y)));
            }

            return asteroids;
        }

        private static Danger GetDanger(int x, int y) => Hash(x, y) switch
        {
            > 0.8 => Danger.None,
            > 0.6 => Danger.Moderate,
            > 0.4 => Danger.Medium,
            _ => Danger.High
        };

        private static double Hash(int x, int y)
        {
            var centerX = mSectorCountWidth / 2;
            var centerY = mSectorCountHeight / 2;
            var center = new Vector2(centerX, centerY);
            var distance = Vector2.Distance(center, new(x, y));
            return distance / Math.Min(centerX, centerY);
        }

        public static Vector2 GetScalingPosition(int x, int y, int scaling)
        {
            var sectorBegin = new Vector2(x, y) * scaling + new Vector2(scaling, scaling) * 0.2f;
            return sectorBegin;
        }

        public static Planet GetPlanet(Vector2 orbitCenter, int oribitRadius, int orbitNumber) => orbitNumber switch
        {
            1 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
            2 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
            3 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Dry(orbitCenter, oribitRadius) }),
            4 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Dry(orbitCenter, oribitRadius), new PlanetTypes.Tessatial(orbitCenter, oribitRadius) }),
            5 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
            6 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Stone(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
            7 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
            8 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
            9 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Cold(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
            10 => new PlanetTypes.Cold(orbitCenter, oribitRadius),
            _ => null
        };
    }
}
