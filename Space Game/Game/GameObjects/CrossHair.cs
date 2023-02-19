using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core.GameObject;
using System;
using System.Buffers;

namespace Space_Game.Game.GameObjects
{
    public class CrossHair : GameObject
    {
        const int textureHeight = 1024;
        const int textureWidth = 1024;

        private float mMaxScale;
        private float mMinScale;
        private float mScale;
        private float mRotation;
        public bool mDrawInnerRing;

        public CrossHair(float minScale, float maxScale, Vector2 position)
        {
            mScale = mMinScale = minScale;
            mMaxScale = maxScale;
            mDrawInnerRing = false;
            Offset = new Vector2(textureWidth, textureHeight) / 2;
            Position = position;
            mRotation = 0;
            TextureHeight = textureHeight;
            TextureWidth = textureWidth;
        }

        public override void Draw()
        {
            var crossHair1 = TextureManager.GetInstance().GetTexture("crossHair1");
            TextureManager.GetInstance().GetSpriteBatch().Draw(crossHair1, Position, null, Color.White,
                0, Offset, mScale, SpriteEffects.None, 0.0f);
            if (!mDrawInnerRing) { return; }
            var crossHair2 = TextureManager.GetInstance().GetTexture("crossHair2");
            TextureManager.GetInstance().GetSpriteBatch().Draw(crossHair2, Position, null, Color.White,
                mRotation, Offset, mScale, SpriteEffects.None, 0.0f);
            mDrawInnerRing = false;
        }


        private void Animation()
        {
            if (Hover)
            {
                if (mScale < mMaxScale)
                {
                    mScale += 0.01f;
                }
            }
            else
            {
                if (mScale > mMinScale)
                {
                    mScale -= 0.01f;
                }
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            Animation();
            mRotation += 0.05f;
        }
    }
}
