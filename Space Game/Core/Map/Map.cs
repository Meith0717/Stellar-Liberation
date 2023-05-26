using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game;
using Galaxy_Explovive.Game.GameLogik;
using Galaxy_Explovive.Game.GameObjects;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy_Explovive.Core.Map
{
    public static class Map
    {
        public static List<PlanetSystem> Generate(GameLayer gameLayer, int SystemAmount, Vector2 MapSize)
        {
            int counter = 0;
            List<PlanetSystem> map = new List<PlanetSystem>();
            while (counter < SystemAmount)
            {
                Vector2 position = MyUtility.GetRandomVector2(-(int)MapSize.X, (int)MapSize.X, -(int)MapSize.Y, (int)MapSize.Y);
                List<Star> neighbourSystem = ObjectLocator.GetObjectsInRadius(gameLayer.mSpatialHashing, 
                    position, Globals.mPlanetSystemDistanceRadius).OfType<Star>().ToList();
                if (neighbourSystem.Count > 0)
                {
                    continue;
                }
                map.Add(new PlanetSystem(gameLayer, position));
                counter++;
            }
            return map;
        }

        public static void DrawGrid(Rectangle MapSize, TextureManager textureManager)
        {
            int ColorAplpha = 20;

            for (float x = -MapSize.X; x <= MapSize.Width / 2; x += 10000)
            {
                textureManager.DrawAdaptiveLine(new Vector2(x, -MapSize.Height / 2f - 10000),
                    new Vector2(x, MapSize.Height / 2f + 10000), new Color(ColorAplpha, ColorAplpha, ColorAplpha, ColorAplpha), 1, 0);
            }

            for (float y = -MapSize.Y; y <= MapSize.Height / 2; y += 10000)
            {
                textureManager.DrawAdaptiveLine(new Vector2(-MapSize.Width / 2f - 10000, y),
                    new Vector2(MapSize.Width / 2f + 10000, y), new Color(ColorAplpha, ColorAplpha, ColorAplpha, ColorAplpha), 1, 0);
            }
        }
    }
}
