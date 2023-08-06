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

        private int mMapWidth = 200;
        private int mMapHeight = 200;
        private int mSectorSize = 500000;

        public void Generate()
        {
            int seed = RandomSeed.Time();
            MapGenerator.Generate(seed, 200, 200, mSectorSize,out mStars,out mPlanets);
        }

        public void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine)
        {
            foreach (var star in mStars)
            {
                if (!gameEngine.FrustumCuller.WorldFrustum.Intersects(star.BoundedBox)) continue;
                star.Update(gameTime, inputState, gameEngine);
            }
        }
    }
}
