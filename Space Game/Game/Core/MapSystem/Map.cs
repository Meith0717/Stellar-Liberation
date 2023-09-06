using CelestialOdyssey.Core.GameEngine.Content_Management;
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
        [JsonProperty] private int mSectorSclae = 20000000;
        [JsonProperty] private int mMapScale = 100;

        public int Height { get { return mSectorCountHeight * mSectorSclae; } }
        public int Width { get { return mSectorCountWidth * mSectorSclae; } }

        public void Generate(GameLayer gameLayer)
        {
            var triangularDistribution = new Triangular(1, 10, 6);
            var noiseMapGenerator = new NoiseMapGenerator(RandomSeed.Time(), mSectorCountWidth, mSectorCountHeight);
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
                    Star star = StarTypes.GenerateRandomStar(GenerateStarPosition(x, y, mSectorSclae));
                    star.AddToSpatialHashing(gameLayer);

                    // Generate Planets of Star
                    var orbitsAmount = (int)triangularDistribution.Sample();
                    var orbitRadius = (int)(star.Width * star.TextureScale * 0.5f);

                    for (int i = 1; i <= orbitsAmount; i++)
                    {
                        orbitRadius += 300000;
                        Planet planet = GetPlanet(star.Position, orbitRadius, i);
                        planet.AddToSpatialHashing(gameLayer);
                        planets.Add(planet);
                    }

                    // Generate Planet System
                    PlanetSystem planetSystem = new(GetMapPosition(star.Position), star.Position, orbitRadius / 2, star.TextureId, star.LightColor);
                    planetSystem.SetAstronomicalObjects(star, planets);
                    mPlanetSystems.Add(planetSystem);
                }
            }
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

        public Vector2 GetMapPosition(Vector2 sectorPosition) { return sectorPosition / mSectorSclae * mMapScale; }
        public Vector2 GetSectorPosition(Vector2 mapPosition) { return mapPosition / mMapScale * mSectorSclae; }
        public PlanetSystem GetRandomSystem() { return Utility.Utility.GetRandomElement(mPlanetSystems); }

        public void DrawSectores(SceneLayer sceneLayer)
        {
            var screen = sceneLayer.FrustumCuller.WorldFrustum;

            var mapWidth = (mSectorCountWidth * mMapScale) + mMapScale;
            var mapHeight = (mSectorCountHeight * mMapScale) + mMapScale;

            for (int x = 0; x < mapWidth; x += mMapScale)
            {
                if (x < screen.X && x > screen.X + screen.Width) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(x, -mMapScale), new(x, mapHeight), new Color(10, 10, 10, 10), 1, 0, sceneLayer.Camera.Zoom);
            }
            for (int y = 0; y < mapHeight; y += mMapScale)
            {
                if (y < screen.Y && y > screen.Y + screen.Height) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(-mMapScale, y), new(mapWidth, y), new Color(10, 10, 10, 10), 1, 0, sceneLayer.Camera.Zoom);
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
