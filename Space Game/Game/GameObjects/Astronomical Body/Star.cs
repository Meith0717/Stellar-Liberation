using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObjects.Types;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
            Width = 1024;
            Height = 1024;
            TextureOffset = new Vector2(Width, Height) / 2;
            TextureScale = mType.Size;
            TextureDepth = 1;
            TextureColor = Color.White;
            mLightColor = mType.StarColor;
            SelectZoom = 1;
            BoundedBox = new CircleF(Position, (Math.Max(Height, Width) / 2) * TextureScale);
        }

        public override void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            AddToSpatialHashing(engine);
            base.UpdateLogic(gameTime, inputState, engine);
            TextureOffset = new Vector2(Width, Height) / 2;
            if (mType == StarTypes.BH) return;
            Rotation += 0.0001f;
        }

        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            textureManager.DrawGameObject(this, IsHover);
            if (mType == StarTypes.BH) return;
            textureManager.Draw("StarLightAlpha", Position, TextureOffset, TextureScale * 1.3f, 0, 2, mLightColor);
        }
    }
}
