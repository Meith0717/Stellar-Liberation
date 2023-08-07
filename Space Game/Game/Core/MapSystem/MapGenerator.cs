using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public static class MapGenerator
    {
        public static void Generate(int seed, int width, int height, int scaling, out List<Star> stars, out List<Planets> planets)
        {
            stars = new List<Star>();
            planets = new List<Planets>();

            NoiseMapGenerator noiseMapGenerator = new(seed, width, height);
            var noiseMap = noiseMapGenerator.GenerateBinaryNoiseMap(40, 6, 5, 0.85, 0);

            int rows = noiseMap.GetLength(0);
            int columns = noiseMap.GetLength(1);

            // Separate x and y coordinates
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (noiseMap[x, y] == 0) continue;
                    stars.Add(GetStar(GenerateStarPosition(x, y, scaling)));
                }
            }
        }

        private static Vector2 GenerateStarPosition(int x, int y, int scaling)
        {
            var sectorBegin = new Vector2(x, y) * scaling;
            var sectorEnd = sectorBegin + new Vector2(scaling, scaling);
            return Utility.GetRandomVector2(sectorBegin, sectorEnd);
        }

        private static Star GetStar(Vector2 position)
        {
            var rnd = Utility.Random.NextDouble();

            return rnd switch
            {
                < 0.01d => new Star(position, ContentRegistry.starBH.Name, Utility.Random.Next(10, 30), Color.Transparent),
                < 0.1d => new Star(position, ContentRegistry.starF.Name, Utility.Random.Next(35, 40), Color.LightBlue),
                < 0.15d => new Star(position, ContentRegistry.starA.Name, Utility.Random.Next(30, 35), Color.Blue),
                < 0.2d => new Star(position, ContentRegistry.starB.Name, Utility.Random.Next(25, 30), Color.Blue),
                < 0.35d => new Star(position, ContentRegistry.starO.Name, Utility.Random.Next(20, 25), Color.DarkBlue),
                < 0.5d => new Star(position, ContentRegistry.starM.Name, Utility.Random.Next(10, 15), Color.OrangeRed),
                < 0.7d => new Star(position, ContentRegistry.starK.Name, Utility.Random.Next(15, 20), Color.Orange),
                < 1d => new Star(position, ContentRegistry.starG.Name, Utility.Random.Next(20, 25), Color.Orange),
                _ => null,
            };
        }
    }
}
