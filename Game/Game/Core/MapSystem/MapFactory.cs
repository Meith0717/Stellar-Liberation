// Map.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.MapSystem
{
    public static class MapFactory
    {
        public readonly static int mSectorCountWidth = 40;
        public readonly static int mSectorCountHeight = 40;
        public readonly static int MapScale = 100;
        public readonly static int ViewScale = 1000000;

        public static void Generate(out HashSet<PlanetSystem> planetSystems)
        {
            planetSystems = new();

            var triangularDistribution = new Triangular(2, 10, 6);
            var noiseMapGenerator = new NoiseMapGenerator(mSectorCountWidth, mSectorCountHeight);
            var noiseMap = noiseMapGenerator.GenerateBinaryNoiseMap();

            int rows = noiseMap.GetLength(0);
            int columns = noiseMap.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (noiseMap[x, y] == 0) continue;

                    // Generate Star
                    var star = StarTypes.GenerateRandomStar(GetScalingPosition(x, y, ViewScale));

                    // Generate Planets of Star
                    var planets = new List<Planet>();
                    var planetAmount = (int)triangularDistribution.Sample();
                    var orbitRadius = star.Width * star.TextureScale;

                    for (int i = 1; i <= planetAmount; i++)
                    {
                        orbitRadius += ExtendetRandom.Random.Next(25000, 40000);
                        planets.Add(GetPlanet(star.Position, (int)orbitRadius, i));
                    }

                    orbitRadius *= 1.5f;

                    // Generate Planet System
                    var danger = GetDanger(x, y);
                    var planetSystem = new PlanetSystem(GetScalingPosition(x, y, MapScale), star.Position, (int)orbitRadius, star.TextureId, star.LightColor, danger);
                    planetSystems.Add(planetSystem);
                     
                    // Generate Quantum Gate

                    planetSystem.SetObjects(star, planets);
                }
            }
        }

        private static int GetPirateAmount(Danger danger) => danger switch
        {
            Danger.None => 1,
            Danger.Moderate => ExtendetRandom.Random.Next(0, 10),
            Danger.Medium => ExtendetRandom.Random.Next(10, 20),
            Danger.High => ExtendetRandom.Random.Next(20, 30),
            _ => throw new NotImplementedException()
        };

        private static Danger  GetDanger(int x, int y) => Hash(x, y) switch
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

        private static Vector2 GetScalingPosition(int x, int y, int scaling)
        {
            var sectorBegin = (new Vector2(x, y) * scaling) + (new Vector2(scaling, scaling) * 0.2f);
            var sectorEnd = sectorBegin + new Vector2(scaling, scaling) * 0.6f;
            return ExtendetRandom.GetRandomVector2(sectorBegin, sectorEnd);
        }

        private static Planet GetPlanet(Vector2 orbitCenter, int oribitRadius, int orbitNumber) => orbitNumber switch
        {
            1 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
            2 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
            3 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Dry(orbitCenter, oribitRadius) }),
            4 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Dry(orbitCenter, oribitRadius), new PlanetTypes.Tessatial(orbitCenter, oribitRadius) }),
            5 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius)}),
            6 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Stone(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
            7 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
            8 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
            9 => ExtendetRandom.GetRandomElement<Planet>(new() { new PlanetTypes.Cold(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
            10 => new PlanetTypes.Cold(orbitCenter, oribitRadius),
            _ => null
        };
    }
}
