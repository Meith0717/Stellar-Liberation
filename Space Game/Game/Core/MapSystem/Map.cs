using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    [Serializable]
    public class Map
    {
        [JsonProperty] private int mSectorCountWidth = (int)(50 * 1.7f);
        [JsonProperty] private int mSectorCountHeight = 50;
        [JsonProperty] private int mSectorSize = 1000000;

        public int Height { get { return mSectorCountHeight * mSectorSize; } }
        public int Width { get { return mSectorCountWidth * mSectorSize; } }

        public void Generate(GameLayer gameLayer)
        {
            NoiseMapGenerator noiseMapGenerator 
                = new(RandomSeed.Time(), mSectorCountWidth, mSectorCountHeight);
            var noiseMap = noiseMapGenerator.GenerateBinaryNoiseMap(40, 6, 5, 0.85, 0);

            SystemGenerator.Generate(noiseMap, mSectorSize, gameLayer);
        }

        public void DrawSectores(Rectangle worldFrustum, float CamZoom)
        {
            var screen = worldFrustum;

            var mapWidth = (mSectorCountWidth * mSectorSize) + mSectorSize;
            var mapHeight = (mSectorCountHeight * mSectorSize) + mSectorSize;

            for (int x = 0; x < mapWidth; x += mSectorSize)
            {
                if (x < screen.X && x > screen.X + screen.Width) continue;
                TextureManager.Instance.DrawAdaptiveLine(new Vector2(x, 0), new Vector2(x, mapHeight), new Color(10, 10, 10, 10), 1, 0, CamZoom);
            }
            for (int y = 0; y < mapHeight; y += mSectorSize)
            {
                if (y < screen.Y && y > screen.Y + screen.Height) continue;
                TextureManager.Instance.DrawAdaptiveLine(new Vector2(0, y), new Vector2(mapWidth, y), new Color(10, 10, 10, 10), 1, 0, CamZoom);
            }
        }
    }
}
