using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Map
{
    public class Map
    {
        private Game.Game mGameLayer;
        private int mWidth;
        private int mHeight;
        private int mPlanetSystemAmount;
        private int mMinDistanceBetweenSystems;

        public List<PlanetSystem> PlanetSystems = new();

        public Map(Game.Game game, int planetSystemAmount, int minDistanceBetwenSystems, int height, int width) 
        { 
            mGameLayer = game;
            mHeight = height;
            mWidth = width;
            mPlanetSystemAmount = planetSystemAmount;
            mMinDistanceBetweenSystems = minDistanceBetwenSystems;
        }

        public double Generate()
        {
            if (PlanetSystems.Count == mPlanetSystemAmount) return 1;
            Vector2 randomPos = MyUtility.GetRandomVector2(0, mWidth, 0, mHeight);
            double percentage = PlanetSystems.Count / (double)mPlanetSystemAmount;

            bool foundIssue=false;
            foreach(PlanetSystem planetSystem in PlanetSystems)
            {
                foundIssue = (Vector2.Distance(randomPos, planetSystem.Position) < mMinDistanceBetweenSystems);
                if (foundIssue) { break; }
            }
            if (foundIssue) return percentage;
            PlanetSystems.Add(new(mGameLayer, randomPos));
            return percentage;
        }

        public void Update(GameTime gameTime, InputState inputState)
        {
            if (PlanetSystems.Count == 0) { throw new System.Exception("No Map was Generated"); }
            foreach (PlanetSystem planetSystem in PlanetSystems)
            {
                planetSystem.UpdateLogik(gameTime, inputState);
            }
        }

        public void Draw()
        {
            foreach (PlanetSystem planetSystem in PlanetSystems)
            {
                planetSystem.Draw();
            }

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
