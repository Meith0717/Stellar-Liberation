// MapFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Random;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration
{
    public static class MapFactory
    {
        public readonly static int mSectorCountWidth = 50;
        public readonly static int mSectorCountHeight = 50;
        public readonly static int MapScale = 100;
        public readonly static int ViewScale = 1000000;

        public static void Generate(out HashSet<PlanetSystem> planetSystems)
        {
            planetSystems = new();
            var seededRandom = new Random(42);

            var noiseMap = BinaryMapGenerator.Generate(mSectorCountWidth, mSectorCountHeight, seededRandom);
            var positions = BinaryMapGenerator.GetVector2sFormBinaryMap(noiseMap);
            BinaryMapGenerator.ScaleVector2s(ref positions, MapScale);
            BinaryMapGenerator.ShiftVector2s(ref positions, 35, seededRandom);

            foreach (var position in positions)
            {
                var planetSystem = new PlanetSystem(position, seededRandom.NextFullRangeInt32());
                planetSystems.Add(planetSystem);
            }
        }
    }
}
