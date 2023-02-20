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
    public class CrossHair
    {
        const int textureHeight = 1024;
        const int textureWidth = 1024;

        private float mMaxScale;
        private float mMinScale;
        private float mScale;
        private float mRotation;
        private Color mColor;
        private Vector2 mPosition;
        private Vector2 mOffset;
        private bool mHover;
        private float mAlphaF;

        public int mAlpha;
        public bool mDrawInnerRing;

        public CrossHair(float minScale, float maxScale, Vector2 position, Color color)
        {
            mScale = mMinScale = minScale;
            mMaxScale = maxScale;
            mDrawInnerRing = false;
            mColor = color;
            mAlpha = 255;
            mOffset = new Vector2(textureWidth, textureHeight) / 2;
            mPosition = position;
            mRotation = 0;
        }

        public void Draw()
        {
            var crossHair1 = TextureManager.GetInstance().GetTexture("crossHair1");
            TextureManager.GetInstance().GetSpriteBatch().Draw(crossHair1, mPosition, null, 
                new Color(mColor.R, mColor.G, mColor.B, mAlpha),
                0, mOffset, mScale, SpriteEffects.None, 0.0f);

            if (!mDrawInnerRing) { return; }
            var crossHair2 = TextureManager.GetInstance().GetTexture("crossHair2");
            TextureManager.GetInstance().GetSpriteBatch().Draw(crossHair2, mPosition, null,
                 new Color(mColor.R, mColor.G, mColor.B, mAlpha),
                 mRotation, mOffset, mScale, SpriteEffects.None, 0.0f);

            mDrawInnerRing = false;
        }


        private void Animation()
        {
            if (mHover)
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

        public void Update(Vector2 position, bool hover)
        {
            Animation();
            mHover = hover;
            mPosition = position;
            mRotation += 0.05f;
            mAlphaF = mAlpha / 255;
        }
    }
}
