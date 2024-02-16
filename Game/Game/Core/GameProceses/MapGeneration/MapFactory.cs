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
        public readonly static int MapScale = 1000;

        public static void Generate(out HashSet<PlanetSystem> planetSystems, MapConfig mapConfig)
        {
            planetSystems = new();
            var seededRandom = new Random(mapConfig.Seed);

            var noiseMap = BinaryMapGenerator.Generate(mapConfig.SectorCountWidth, mapConfig.SectorCountHeight, seededRandom);
            var positions = BinaryMapGenerator.GetVector2sFormBinaryMap(noiseMap);
            BinaryMapGenerator.ScaleVector2s(ref positions, MapScale);
            BinaryMapGenerator.ShiftVector2s(ref positions, 350, seededRandom);

            foreach (var position in positions)
            {
                var planetSystem = new PlanetSystem(position, seededRandom.NextFullRangeInt32());
                planetSystems.Add(planetSystem);
            }
        }
    }
}
