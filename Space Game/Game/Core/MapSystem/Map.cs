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
        [JsonProperty] private int mSectorCountWidth = 20;
        [JsonProperty] private int mSectorCountHeight = 20;
        [JsonProperty] private int mSectorSize = 150;

        public int Height { get { return mSectorCountHeight * mSectorSize; } }
        public int Width { get { return mSectorCountWidth * mSectorSize; } }
        public Vector2 Middle { get { return new(Width / 2, Height / 2); } }

        public void Generate(GameLayer gameLayer)
        {
            NoiseMapGenerator noiseMapGenerator 
                = new(RandomSeed.Time(), mSectorCountWidth, mSectorCountHeight);
            var noiseMap = noiseMapGenerator.GenerateBinaryNoiseMap(40, 6, 5, 0.85, 0);

            SystemGenerator.Generate(noiseMap, mSectorSize, gameLayer, out var stars);

            foreach (var star in stars)
            {
                star.GetnNighbors(Utility.Utility.Random.Next(2, 5));
            }
        }
    }
}
