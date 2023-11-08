// Map.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.MapSystem
{
    [Serializable]
    public class Map
    {
        [JsonProperty] private int mSectorCountWidth = 40;
        [JsonProperty] private int mSectorCountHeight = 40;
        [JsonProperty] public readonly int MapScale = 100;

        public void Generate(Player player, out HashSet<PlanetSystem> planetSystems)
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
                    var planets = new List<Planet>();

                    // Generate Star
                    Star star = StarTypes.GenerateRandomStar(Vector2.Zero);

                    // Generate Planets of Star
                    var orbitsAmount = (int)triangularDistribution.Sample();
                    var orbitRadius = star.Width * star.TextureScale * 0.5f;

                    for (int i = 1; i <= orbitsAmount; i++)
                    {
                        orbitRadius += ExtendetRandom.Random.Next(25000, 40000);
                        Planet planet = GetPlanet(star.Position, (int)orbitRadius, i);
                        planets.Add(planet);
                    }

                    orbitRadius *= 1.5f;

                    // Generate Planet System
                    var danger = GetDanger(x, y);
                    PlanetSystem planetSystem = new(GenerateStarPosition(x, y, MapScale), star.Position, (int)orbitRadius, star.TextureId, star.LightColor, danger);
                    planetSystems.Add(planetSystem);
                     
                    // Generate Quantum Gate
                    var quantumGate = new QuantumGate(ExtendetRandom.NextVectorOnBorder(planetSystem.SystemBounding));

                    planetSystem.SetObjects(star, planets, player, null, quantumGate);
                }
            }
        }

        private int GetPirateAmount(Danger danger) => danger switch
        {
            Danger.None => 1,
            Danger.Moderate => ExtendetRandom.Random.Next(0, 10),
            Danger.Medium => ExtendetRandom.Random.Next(10, 20),
            Danger.High => ExtendetRandom.Random.Next(20, 30),
            _ => throw new NotImplementedException()
        };

        private Danger GetDanger(int x, int y) => Hash(x, y) switch
        {
            > 0.8 => Danger.None,
            > 0.6 => Danger.Moderate,
            > 0.4 => Danger.Medium,
            _ => Danger.High
        };

        private double Hash(int x, int y)
        {
            var centerX = mSectorCountWidth / 2;
            var centerY = mSectorCountHeight / 2;
            var center = new Vector2(centerX, centerY);
            var distance = Vector2.Distance(center, new(x, y));
            return distance / Math.Min(centerX, centerY);
        }

        private static Vector2 GenerateStarPosition(int x, int y, int scaling)
        {
            var sectorBegin = (new Vector2(x, y) * scaling) + (new Vector2(scaling, scaling) * 0.2f);
            var sectorEnd = sectorBegin + new Vector2(scaling, scaling) * 0.6f;
            return ExtendetRandom.GetRandomVector2(sectorBegin, sectorEnd);
        }

        private static Planet GetPlanet(Vector2 orbitCenter, int oribitRadius, int orbitNumber)
        {
            return orbitNumber switch
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

        public void DrawSectores(Scene scene)
        {
            var screen = scene.FrustumCuller.WorldFrustum;

            var mapWidth = (mSectorCountWidth * MapScale) + MapScale;
            var mapHeight = (mSectorCountHeight * MapScale) + MapScale;

            for (int x = 0; x < mapWidth; x += MapScale)
            {
                if (x < screen.X && x > screen.X + screen.Width) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(x, -MapScale), new(x, mapHeight), new Color(10, 10, 10, 10), 1, 0, scene.Camera.Zoom);
            }
            for (int y = 0; y < mapHeight; y += MapScale)
            {
                if (y < screen.Y && y > screen.Y + screen.Height) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(-MapScale, y), new(mapWidth, y), new Color(10, 10, 10, 10), 1, 0, scene.Camera.Zoom);
            }
        }

    }
}
