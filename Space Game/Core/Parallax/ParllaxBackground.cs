﻿using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Game;

namespace Galaxy_Explovive.Core.Effects
{
    internal class ParllaxBackground
    {
        private readonly string mTextureId;
        private int mTextureWidth;
        private int mTextureHeight;

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
            mTextureWidth = GameGlobals.GraphicsDevice.Viewport.Width;
            mTextureHeight = GameGlobals.GraphicsDevice.Viewport.Height;
        }

        public void Update(Vector2 movement)
        {
            if (mMovingScale <= 0) { return; }
            ManagMovement(movement);
            ManagerCorners();
        }

        public void Draw(TextureManager textureManager)
        {
            textureManager.Draw(mTextureId, mPosition1, mTextureWidth, mTextureHeight);
            textureManager.Draw(mTextureId, mPosition2, mTextureWidth, mTextureHeight);
            textureManager.Draw(mTextureId, mPosition3, mTextureWidth, mTextureHeight);
            textureManager.Draw(mTextureId, mPosition4, mTextureWidth, mTextureHeight);
            textureManager.Draw(mTextureId, mPosition5, mTextureWidth, mTextureHeight);
        }

        public void OnResolutionChanged(GraphicsDevice graphicsDevice)
        {
            mTextureWidth = graphicsDevice.Viewport.Width;
            mTextureHeight = graphicsDevice.Viewport.Height;
        }

        private void ManagMovement(Vector2 movement)
        {
            mPosition1 -= movement * mMovingScale;

        }

        private void ManagerCorners()
        {
            mPosition1.X %= mTextureWidth;
            mPosition1.Y %= mTextureHeight;

            mPosition2 = new Vector2(mPosition1.X - mTextureWidth, mPosition1.Y);
            if (mPosition1.X < 0)
            {
                mPosition2 = new Vector2(mPosition1.X + mTextureWidth, mPosition1.Y);
            }

            mPosition3 = new Vector2(mPosition1.X, mPosition1.Y - mTextureHeight);
            if (mPosition1.Y < 0)
            {
                mPosition3 = new Vector2(mPosition1.X, mPosition1.Y + mTextureHeight);
            }

            mPosition4 = new Vector2(mPosition1.X - mTextureWidth, mPosition1.Y - mTextureHeight);
            if (mPosition1.X < 0 && mPosition1.Y < 0)
            {
                mPosition4 = new Vector2(mPosition1.X + mTextureWidth, mPosition1.Y + mTextureHeight);
            }

            mPosition5 = new Vector2(mPosition1.X + mTextureWidth, mPosition1.Y - mTextureHeight);
            if (mPosition1.Y < 0)
            {
                mPosition5 = new Vector2(mPosition1.X - mTextureWidth, mPosition1.Y + mTextureHeight);
            }
        }
    }
}
