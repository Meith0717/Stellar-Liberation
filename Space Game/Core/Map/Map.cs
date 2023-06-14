using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.Map
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
            if (SectorAmount % 2 != 0) { throw new Exception("Sector amount has to be odd"); }
            mWidth = mHeight = edgeLength;
            SectorSize = (edgeLength == 0)? 0 : edgeLength/(SectorAmount/2);
        }

        public void Generate()
        {
            for (int x = -mWidth / 2; x <= mWidth / 2; x += SectorSize)
            {
                for (int y = -mHeight / 2; y <= mHeight / 2; y += SectorSize)
                {
                    if (MyUtility.Random.NextDouble() < 0.3) { continue; }
                    Vector2 randomPos = MyUtility.GetRandomVector2(x + 2000, x+SectorSize - 2000,
                        y + 2000, y+SectorSize -2000);
                    PlanetSystems.Add(new(randomPos));
                }
            }
            System.Diagnostics.Debug.WriteLine(PlanetSystems.Count);
        }

        public void Update(GameTime gameTime, InputState inputState)
        {
            if (PlanetSystems.Count == 0) { throw new System.Exception("No Map was Generated"); }
            foreach (PlanetSystem planetSystem in PlanetSystems)
            {
                planetSystem.UpdateLogik(gameTime, inputState);
            }
        }

        public void Draw(TextureManager textureManager)
        {
            foreach (PlanetSystem planetSystem in PlanetSystems)
            {
                planetSystem.Draw(textureManager);
            }

        }

        public void DrawGrid(TextureManager textureManager)
        {
            int ColorAplpha = 20;
            Color color = new Color(ColorAplpha, ColorAplpha, ColorAplpha);

            for (int x = -mWidth/2; x <= mWidth/2 + SectorSize; x+=SectorSize)
            {
                textureManager.DrawAdaptiveLine(new(x, -mHeight / 2 + SectorSize), new(x, mHeight / 2 + SectorSize), color, 1, 0);
            }

            for (int y = -mHeight / 2; y <= mHeight / 2 + SectorSize; y += SectorSize)
            {
                textureManager.DrawAdaptiveLine(new(-mWidth / 2 + SectorSize, y), new(mWidth / 2 + SectorSize, y), color, 1, 0);
            }

        }
    }
}
