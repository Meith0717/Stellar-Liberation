// ParllaxBackground.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;

namespace StellarLiberation.Game.Core.Visuals.ParallaxSystem
{
    internal class ParllaxBackground
    {
        private readonly string mTextureId;
        private float mTextureWidth;
        private float mTextureHeight;

        private Vector2 mPosition1;
        private Vector2 mPosition2;
        private Vector2 mPosition3;
        private Vector2 mPosition4;
        private Vector2 mPosition5;

        private readonly float mMovingScale;

        public ParllaxBackground(string textureId, float movingScale)
        {
            mTextureId = textureId;
            mMovingScale = movingScale;
            mPosition1 = Vector2.Zero;
            mTextureWidth = 1920f;
            mTextureHeight = 1080f;
        }

        public void Update(Vector2 movement)
        {
            if (mMovingScale <= 0) { return; }
            mPosition1 -= movement * mMovingScale;

            mPosition1.X %= mTextureWidth;
            mPosition1.Y %= mTextureHeight;

            mPosition2 = new Vector2(mPosition1.X - mTextureWidth, mPosition1.Y);
            if (mPosition1.X < 0) mPosition2 = new Vector2(mPosition1.X + mTextureWidth, mPosition1.Y);

            mPosition3 = new Vector2(mPosition1.X, mPosition1.Y - mTextureHeight);
            if (mPosition1.Y < 0) mPosition3 = new Vector2(mPosition1.X, mPosition1.Y + mTextureHeight);

            mPosition4 = new Vector2(mPosition1.X - mTextureWidth, mPosition1.Y - mTextureHeight);
            if (mPosition1.X < 0 && mPosition1.Y < 0) mPosition4 = new Vector2(mPosition1.X + mTextureWidth, mPosition1.Y + mTextureHeight);

            mPosition5 = new Vector2(mPosition1.X + mTextureWidth, mPosition1.Y - mTextureHeight);
            if (mPosition1.Y < 0) mPosition5 = new Vector2(mPosition1.X - mTextureWidth, mPosition1.Y + mTextureHeight);
        }

        public void Draw()
        {
            TextureManager.Instance.Draw(mTextureId, mPosition1, mTextureWidth, mTextureHeight, Color.White);
            TextureManager.Instance.Draw(mTextureId, mPosition2, mTextureWidth, mTextureHeight, Color.White);
            TextureManager.Instance.Draw(mTextureId, mPosition3, mTextureWidth, mTextureHeight, Color.White);
            TextureManager.Instance.Draw(mTextureId, mPosition4, mTextureWidth, mTextureHeight, Color.White);
            TextureManager.Instance.Draw(mTextureId, mPosition5, mTextureWidth, mTextureHeight, Color.White);
        }

        public void OnResolutionChanged(GraphicsDevice graphicsDevice)
        {
            mTextureWidth = graphicsDevice.Viewport.Width;
            mTextureHeight = graphicsDevice.Viewport.Height;
        }
    }
}
