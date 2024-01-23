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
            var seededRandom = new Random(5423);

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
    }
}
