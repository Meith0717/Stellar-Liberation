using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Core.GameEngine.GameObjects;
using CelestialOdyssey.Core.GameEngine.InputManagement;
using CelestialOdyssey.Core.GameEngine.Utility;
using CelestialOdyssey.Game.GameObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Core.Map
{
    [Serializable]
    public class Map
    {
        [JsonProperty] private readonly int mWidth;
        [JsonProperty] private readonly int mHeight;

        [JsonProperty] public List<Vector2> SectorList = new();
        [JsonProperty] public int SectorSize;
        [JsonProperty] public List<PlanetSystem> PlanetSystems = new();

        [JsonProperty] private int mSectorGridMode;

        public Map(int edgeLength, int SectorAmount)
        {
            if (Math.Sqrt(SectorAmount) % 1 != 0) { throw new Exception("Square Root of Sector amount has to be Integer"); }
            mWidth = mHeight = edgeLength;
            SectorSize = (edgeLength == 0) ? 0 : edgeLength / (int)Math.Sqrt(SectorAmount);
            mSectorGridMode = 3;
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
                    PlanetSystem system = new (randomPos);
                    PlanetSystems.Add(system);
                }
            }
        }

        public void Update(GameTime time, InputState input, GameEngine.GameEngine engine)
        {
            if (input.mActionList.Contains(ActionType.ToggleSectorGrid))
            {
                mSectorGridMode += mSectorGridMode < 3 ? 1 : -mSectorGridMode;
            }

            foreach (PlanetSystem system in PlanetSystems)
            {
                if (!engine.FrustumCuller.CircleOnWorldView(system.BoundedBox)) continue;
                system.Update(time, input, engine);
            }
        }

        private void ManageSystemVisibility(Camera camera)
        {
            switch (camera.Zoom)
            {
                case <= 0.5f:
                    foreach (PlanetSystem system in PlanetSystems) { system.Show(); }
                    break;
                case > 0.5f:
                    foreach (PlanetSystem system in PlanetSystems) { system.Hide(); }
                    break;
            } 
        }

        private void DrawGrid(TextureManager textureManager, GameEngine.GameEngine engine, Color color)
        {
            for (int x = 0; x <= mWidth + SectorSize; x += SectorSize)
            {
                textureManager.DrawAdaptiveLine(new(x, 0), new(x, mHeight + SectorSize), color, 1, 0, engine.Camera.Zoom);
            }

            for (int y = 0; y <= mHeight + SectorSize; y += SectorSize)
            {
                textureManager.DrawAdaptiveLine(new(0, y), new(mWidth + SectorSize, y), color, 1, 0, engine.Camera.Zoom);
            }
            if (mSectorGridMode >= 2)
            {
                
            }
        }

        private void DrawSectorInfo(TextureManager textureManager, GameEngine.GameEngine engine, Color color)
        {
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
        }

        public void Draw(TextureManager textureManager, GameEngine.GameEngine engine)
        {
            int ColorAplpha = 30;
            Color color = new(ColorAplpha, ColorAplpha, ColorAplpha);

            switch (mSectorGridMode)
            {
                case 0:
                    break;
                case 1:
                    DrawGrid(textureManager, engine, color);
                    break;
                case 2:
                    DrawGrid(textureManager, engine, color);
                    DrawSectorInfo(textureManager, engine, color);
                    break;
                case 3:
                    DrawGrid(textureManager, engine, color);
                    DrawSectorInfo(textureManager, engine, color);
                    break;
            }
        }
    }
}
