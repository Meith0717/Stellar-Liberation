// MapFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration
{
    public static class MapFactory
    {
        public readonly static int mSectorCountWidth = 100;
        public readonly static int mSectorCountHeight = 100;
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
                var planetSystem = new PlanetSystem(position, seededRandom.NextFullRangeInt32());
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
    }
}
