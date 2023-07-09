﻿using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.InputManagement;

using GalaxyExplovive.Core.GameEngine.Utility;
using GalaxyExplovive.Game.GameObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GalaxyExplovive.Core.Map
{
    [Serializable]
    public class Map
    {
        [JsonProperty] private readonly int mWidth;
        [JsonProperty] private readonly int mHeight;

        [JsonProperty] public List<Vector2> SectorList = new();
        [JsonProperty] public int SectorSize;
        [JsonProperty] public List<PlanetSystem> PlanetSystems = new();

        public Map(int edgeLength, int SectorAmount)
        {
            if (Math.Sqrt(SectorAmount) % 1 != 0) { throw new Exception("Square Root of Sector amount has to be Integer"); }
            mWidth = mHeight = edgeLength;
            SectorSize = (edgeLength == 0) ? 0 : edgeLength / (int)Math.Sqrt(SectorAmount);
        }

        public void Generate()
        {
            for (int x = 0; x <= mWidth; x += SectorSize)
            {
                for (int y = 0; y <= mHeight; y += SectorSize)
                {
                    if (Utility.Random.NextDouble() < 0.3) { continue; }
                    Vector2 randomPos = Utility.GetRandomVector2(x + 2000, x + SectorSize - 2000,
                        y + 2000, y + SectorSize - 2000);
                    PlanetSystems.Add(new(randomPos));
                }
            }
        }

        public void Update(GameTime time, InputState input, GameEngine.GameEngine engine)
        {
            foreach (PlanetSystem system in PlanetSystems)
            {
                if (!engine.FrustumCuller.CircleOnWorldView(system.BoundedBox)) continue;
                system.Update(time, input, engine);
            }
        }

        public void DrawGrid(TextureManager textureManager, GameEngine.GameEngine engine)
        {
            int ColorAplpha = 30;
            Color color = new(ColorAplpha, ColorAplpha, ColorAplpha);
            var size = 0.5f / engine.Camera.Zoom;
            size = (size >= 60) ? 60 : size;
            for (int i = 0; i <= mWidth / SectorSize; i++)
            {
                var x = i * SectorSize;
                for (int j = 0; j <= mHeight / SectorSize; j++)
                {
                    var y = j * SectorSize;
                    if (!engine.FrustumCuller.VectorOnWorldView(new(x, y))) continue;
                    textureManager.DrawString("title", new(x + 25, y + 15), $"{i}, {j}", size, color);
                }
            }

            for (int x = 0; x <= mWidth + SectorSize; x += SectorSize)
            {
                textureManager.DrawAdaptiveLine(new(x, 0), new(x, mHeight + SectorSize), color, 1, 0, engine.Camera.Zoom);
            }

            for (int y = 0; y <= mHeight + SectorSize; y += SectorSize)
            {
                textureManager.DrawAdaptiveLine(new(0, y), new(mWidth + SectorSize, y), color, 1, 0, engine.Camera.Zoom);
            }
        }

        public void Draw(TextureManager textureManager, GameEngine.GameEngine engine)
        {
            foreach (PlanetSystem system in PlanetSystems)
            {
                system.Draw(textureManager, engine);
            }
        }
    }
}
