﻿using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public class Map
    {
        private List<Star> mStars = new();
        private List<Planet> mPlanets = new();

        private int mSectorCountWidth = (int)(50 * 1.7f);
        private int mSectorCountHeight = 50;
        private int mSectorSize = 1000000;

        public int Height { get { return mSectorCountHeight * mSectorSize; } }
        public int Width { get { return mSectorCountWidth * mSectorSize; } }

        public void Generate(GameEngine.GameEngine gameEngine)
        {
            NoiseMapGenerator noiseMapGenerator 
                = new(RandomSeed.Time(), mSectorCountWidth, mSectorCountHeight);
            var noiseMap = noiseMapGenerator.GenerateBinaryNoiseMap(40, 6, 5, 0.85, 0);

            StarGenerator.Generate(noiseMap, mSectorSize, gameEngine, out mStars);
            PlanetGenerator.Generate(mStars, gameEngine, out mPlanets);
        }

        public void DrawSectores(GameEngine.GameEngine gameEngine)
        {
            var screen = gameEngine.FrustumCuller.WorldFrustum;

            var mapWidth = (mSectorCountWidth * mSectorSize) + mSectorSize;
            var mapHeight = (mSectorCountHeight * mSectorSize) + mSectorSize;

            for (int x = 0; x < mapWidth; x += mSectorSize)
            {
                if (x < screen.X && x > screen.X + screen.Width) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(x, 0), new(x, mapHeight), new Color(10, 10, 10, 10), 1, 0, gameEngine.Camera.Zoom);
            }
            for (int y = 0; y < mapHeight; y += mSectorSize)
            {
                if (y < screen.Y && y > screen.Y + screen.Height) continue;
                TextureManager.Instance.DrawAdaptiveLine(new(0, y), new(mapWidth, y), new Color(10, 10, 10, 10), 1, 0, gameEngine.Camera.Zoom);
            }
        }
    }
}