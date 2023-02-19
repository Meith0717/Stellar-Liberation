using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using rache_der_reti.Core.Animation;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core.GameObject;
using System;
using System.Buffers;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class CrossHair : GameObject
    {
        const int textureHeight = 1024;
        const int textureWidth = 1024;

        [JsonProperty] private float mScale;
        [JsonProperty] private float mRotation;

        public CrossHair(float scale, Vector2 position)
        {
            mScale = scale;
            Offset = new Vector2(textureWidth, textureHeight) / 2;
            Position = position;
            TextureHeight = textureHeight;
            TextureWidth = textureWidth;
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public void DrawCrossHair1()
        {
            var crossHair1 = TextureManager.GetInstance().GetTexture("crossHair1");
            TextureManager.GetInstance().GetSpriteBatch().Draw(crossHair1, Position, null, Color.White,
                0, Offset, mScale, SpriteEffects.None, 0.0f);
        }

        public void DrawCrossHair2()
        {
            var crossHair2 = TextureManager.GetInstance().GetTexture("crossHair2");
            TextureManager.GetInstance().GetSpriteBatch().Draw(crossHair2, Position, null, Color.White,
                mRotation, Offset, mScale, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mRotation += 0.05f;
        }
    }
}
