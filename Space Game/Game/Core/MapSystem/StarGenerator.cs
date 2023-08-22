using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public static class StarGenerator
    {
        public static void Generate(int[,] map, int scaling, GameEngine.GameEngine gameEngine, out List<Star> stars)
        {
            stars = new();

            int rows = map.GetLength(0);
            int columns = map.GetLength(1);

            // Separate x and y coordinates
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (map[x, y] == 0) continue;
                    Star star = GetStar(GenerateStarPosition(x, y, scaling));
                    star.AddToSpatialHashing(gameEngine);
                    stars.Add(star);
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
                < 0.01d => new Star(position, ContentRegistry.starBH, Utility.Random.Next(10, 30), Color.Transparent),
                < 0.1d => new Star(position, ContentRegistry.starF, Utility.Random.Next(35, 40), Color.LightBlue),
                < 0.15d => new Star(position, ContentRegistry.starA, Utility.Random.Next(30, 35), Color.Blue),
                < 0.2d => new Star(position, ContentRegistry.starB, Utility.Random.Next(25, 30), Color.Blue),
                < 0.35d => new Star(position, ContentRegistry.starO, Utility.Random.Next(20, 25), Color.DarkBlue),
                < 0.5d => new Star(position, ContentRegistry.starM, Utility.Random.Next(10, 15), Color.OrangeRed),
                < 0.7d => new Star(position, ContentRegistry.starK, Utility.Random.Next(15, 20), Color.Orange),
                < 1d => new Star(position, ContentRegistry.starG, Utility.Random.Next(20, 25), Color.Orange),
                _ => null,
            };
        }
    }
}
