using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public class Map
    {
        private List<Star> mStars = new();
        private List<Planets> mPlanets = new();

        private int mSectorCountWidth = 100;
        private int mSectorCountHeight = 100;
        private int mSectorSize = 1000000;

        public void Generate()
        {
            int seed = RandomSeed.Time();
            MapGenerator.Generate(seed, 100 * 2, 100 * 2, mSectorSize / 2,out mStars,out mPlanets);
        }

        public void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine)
        {
            foreach (var star in mStars)
            {
                if (!gameEngine.FrustumCuller.WorldFrustum.Intersects(star.BoundedBox)) continue;
                star.Update(gameTime, inputState, gameEngine);
            }
        }

        public void DrawSectores(GameEngine.GameEngine gameEngine)
        {
            var screen = gameEngine.FrustumCuller.WorldFrustum;

            var mapWidth = (mSectorCountWidth * mSectorSize) + mSectorSize;
            var mapHeight = (mSectorCountHeight * mSectorSize) + mSectorSize;

            for (int x = 0; x < mapWidth; x += mSectorSize)
            {
                if (x < screen.X && x > screen.X + screen.Width) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(x, 0), new(x, mapHeight), Color.Gray, 1, 0, gameEngine.Camera.Zoom);
            }
            for (int y = 0; y < mapHeight; y += mSectorSize)
            {
                if (y < screen.Y && y > screen.Y + screen.Height) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(0, y), new(mapWidth, y), Color.Gray, 1, 0, gameEngine.Camera.Zoom);
            }
        }
    }
}
