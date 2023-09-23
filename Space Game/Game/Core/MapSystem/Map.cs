using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.Layers;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    [Serializable]
    public class Map
    {
        [JsonProperty] public List<PlanetSystem> mPlanetSystems { get; private set; } = new();

        [JsonProperty] private int mSectorCountWidth = 40;
        [JsonProperty] private int mSectorCountHeight = 40;
        [JsonProperty] public readonly int SectorSclae = 20000000;
        [JsonProperty] public readonly int MapScale = 100;

        public int Height { get { return mSectorCountHeight * SectorSclae; } }
        public int Width { get { return mSectorCountWidth * SectorSclae; } }

        public void Generate(GameLayer gameLayer)
        {
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
                    Star star = StarTypes.GenerateRandomStar(GenerateStarPosition(x, y, SectorSclae));
                    star.AddToSpatialHashing(gameLayer);

                    // Generate Planets of Star
                    var orbitsAmount = (int)triangularDistribution.Sample();
                    var orbitRadius = (int)(star.Width * star.TextureScale * 0.5f);

                    for (int i = 1; i <= orbitsAmount; i++)
                    {
                        orbitRadius += 250000;
                        Planet planet = GetPlanet(star.Position, orbitRadius, i);
                        planet.AddToSpatialHashing(gameLayer);
                        planets.Add(planet);
                    }

                    // Generate Planet System
                    var danger = GetDanger(x, y);
                    PlanetSystem planetSystem = new(GetMapPosition(star.Position), star.Position, orbitRadius, star.TextureId, star.LightColor, danger);
                    mPlanetSystems.Add(planetSystem);

                    // Generate Pirates 
                    var pirates = new List<Enemy>();
                    var pirateAmount = GetPirateAmount(danger);
                    for (int i = 0; i < pirateAmount; i++)
                    {
                        Enemy pirate = new(Utility.Utility.GetRandomVector2(planetSystem.SystemBounding));
                        pirates.Add(pirate);
                        pirate.AddToSpatialHashing(gameLayer);
                    }

                    planetSystem.SetObjects(star, planets, pirates);
                }
            }
        }

        private int GetPirateAmount(Danger danger) => danger switch
        {
            Danger.None => 1,
            Danger.Moderate => Utility.Utility.Random.Next(0, 10),
            Danger.Medium => Utility.Utility.Random.Next(10, 20),
            Danger.High => Utility.Utility.Random.Next(20, 30),
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
            return Utility.Utility.GetRandomVector2(sectorBegin, sectorEnd);
        }

        private static Planet GetPlanet(Vector2 orbitCenter, int oribitRadius, int orbitNumber)
        {
            return orbitNumber switch
            {
                1 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
                2 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Warm(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
                3 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Dry(orbitCenter, oribitRadius) }),
                4 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Dry(orbitCenter, oribitRadius), new PlanetTypes.Tessatial(orbitCenter, oribitRadius) }),
                5 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Tessatial(orbitCenter, oribitRadius), new PlanetTypes.Stone(orbitCenter, oribitRadius) }),
                6 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Stone(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
                7 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
                8 => new PlanetTypes.Gas(orbitCenter, oribitRadius),
                9 => Utility.Utility.GetRandomElement<Planet>(new() { new PlanetTypes.Cold(orbitCenter, oribitRadius), new PlanetTypes.Gas(orbitCenter, oribitRadius) }),
                10 => new PlanetTypes.Cold(orbitCenter, oribitRadius),
                _ => null
            };
        }

        public Vector2 GetMapPosition(Vector2 sectorPosition) { return sectorPosition / SectorSclae * MapScale; }
        
        public Vector2 GetSectorPosition(Vector2 mapPosition) { return mapPosition / MapScale * SectorSclae; }
        
        public PlanetSystem GetRandomSystem() { return Utility.Utility.GetRandomElement(mPlanetSystems); }
        
        public void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {
            foreach (var system in mPlanetSystems) { system.Update(gameTime, inputState, gameLayer); }
        }

        public void DrawSectores(SceneLayer sceneLayer)
        {
            var screen = sceneLayer.FrustumCuller.WorldFrustum;

            var mapWidth = (mSectorCountWidth * MapScale) + MapScale;
            var mapHeight = (mSectorCountHeight * MapScale) + MapScale;

            for (int x = 0; x < mapWidth; x += MapScale)
            {
                if (x < screen.X && x > screen.X + screen.Width) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(x, -MapScale), new(x, mapHeight), new Color(10, 10, 10, 10), 1, 0, sceneLayer.Camera.Zoom);
            }
            for (int y = 0; y < mapHeight; y += MapScale)
            {
                if (y < screen.Y && y > screen.Y + screen.Height) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(-MapScale, y), new(mapWidth, y), new Color(10, 10, 10, 10), 1, 0, sceneLayer.Camera.Zoom);
            }
        }

        public PlanetSystem GetActualPlanetSystem(Player player)
        {
            foreach (var item in mPlanetSystems)
            {
                if (!item.CheckIfHasPlayer(player)) continue;
                return item;
                
            }
            return null;
        }


    }
}
