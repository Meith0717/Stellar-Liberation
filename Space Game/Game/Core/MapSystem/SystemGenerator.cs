using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public static class SystemGenerator
    {
        public static void Generate(int[,] map, int scaling, GameLayer gameLayer)
        {
            int rows = map.GetLength(0);
            int columns = map.GetLength(1);

            // Separate x and y coordinates
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (map[x, y] == 0) continue;
                    SolarSystem star = new(GenerateStarPosition(x, y, scaling));
                    star.Initialize(gameLayer);
                    star.AddToSpatialHashing();
                    gameLayer.AddObject(star);
                }
            }
        }

        private static Vector2 GenerateStarPosition(int x, int y, int scaling)
        {
            var sectorBegin = new Vector2(x, y) * scaling;
            var sectorEnd = sectorBegin + new Vector2(scaling, scaling);
            return Utility.Utility.GetRandomVector2(sectorBegin, sectorEnd);
        }
    }
}
