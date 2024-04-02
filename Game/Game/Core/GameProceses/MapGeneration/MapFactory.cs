// MapFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration
{
    public static class MapFactory
    {
        public readonly static int MapScale = 1000;

        public static List<PlanetsystemState> Generate(MapConfig mapConfig)
        {
            var planetSystems = new List<PlanetsystemState>();
            var seededRandom = new Random(mapConfig.Seed);

            var noiseMap = BinaryMapGenerator.Generate(mapConfig.SectorCountWidth, mapConfig.SectorCountHeight, seededRandom);
            var positions = BinaryMapGenerator.GetVector2sFormBinaryMap(noiseMap);
            BinaryMapGenerator.ScaleVector2s(ref positions, MapScale);
            BinaryMapGenerator.ShiftVector2s(ref positions, 350, seededRandom);

            foreach (var position in positions)
            {
                var seed = seededRandom.NextFullRangeInt32();
                planetSystems.Add(new(position, GenerateSystem(seed)));
            }
            return planetSystems;
        }

        public static List<GameObject2D> GenerateSystem(int seed)
        {
            var lst = new List<GameObject2D>();
            var seededRandom = new Random(seed);

            var star = StarGenerator.Generat(seededRandom);

            lst.Add(star);
            var distanceToStar = (int)star.BoundedBox.Radius;
            var planetCount = (int)Triangular.Sample(seededRandom, 1, 10, 7);
            for (int i = 1; i <= planetCount; i++)
            {
                distanceToStar += seededRandom.Next(4000, 10000);
                lst.Add(PlanetGenerator.GetPlanet(seededRandom, star.Kelvin, distanceToStar));
            }

            return lst;
        }

        public static List<PlanetSystem> GetPlanetSystems(List<PlanetsystemState> planetsystemStates)
        {
            var lst = new List<PlanetSystem>();
            foreach (var state in planetsystemStates)
                lst.Add(new PlanetSystem(state));
            return lst;
        }

    }
}
