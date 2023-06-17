using Galaxy_Explovive.Core.GameObjects.Types;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    [Serializable]
    public class Star : AstronomicalBody
    {
        [JsonProperty] public Color mLightColor;
        [JsonProperty] public StarType mType;

        public Star(Vector2 position) : base()
        {
            int randIndex = MyUtility.Random.Next(StarTypes.Types.Count);
            mType = StarTypes.Types[randIndex];
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureId = mType.Texture;
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureScale = mType.Size;
            TextureDepth = 1;
            TextureColor = Color.White;
            mLightColor = mType.StarColor;

            // Add To Spatial Hashing
            GameGlobals.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public override void SelectActions(InputState inputState)
        {
            base.SelectActions(inputState);
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            if (mType == StarTypes.BH) return;
            Rotation += 0.0001f;
        }

        public override void Draw(TextureManager textureManager)
        {
            base.Draw(textureManager);
            textureManager.DrawGameObject(this, IsHover);
            GameGlobals.DebugSystem.DrawBoundBox(textureManager, BoundedBox);
            if (mType == StarTypes.BH) return;
            textureManager.Draw("StarLightAlpha", Position, TextureOffset, TextureScale * 1.3f, 0, 2, mLightColor);
        }
    }
}
