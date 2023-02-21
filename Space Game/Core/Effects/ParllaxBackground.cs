using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;

namespace Space_Game.Core.Effects
{
    internal class ParllaxBackground
    {
        private string mTextureId;
        private int mTextureWidth;
        private int mTextureHeight;

        private Vector2 mPosition1;
        private Vector2 mPosition2;
        private Vector2 mPosition3;
        private Vector2 mPosition4;
        private Vector2 mPosition5;

        private float mDepth;
        private float mMovingScale;

        public ParllaxBackground(string textureId, float depth, float movingScale)
        {
            mTextureWidth = Globals.mGraphicsDevice.Viewport.Width;
            mTextureHeight = Globals.mGraphicsDevice.Viewport.Height;
            mTextureId = textureId;
            mDepth = depth;
            mMovingScale = movingScale;
            mPosition1 = Vector2.Zero;
        }

        public void Update(Vector2 movement)
        {
            if (mMovingScale <= 0) { return; }
            ManagMovement(movement);
            ManagerCorners();
        }

        public void Draw()
        {
            TextureManager.GetInstance().Draw(mTextureId, mPosition1, mTextureWidth, mTextureHeight);
            TextureManager.GetInstance().Draw(mTextureId, mPosition2, mTextureWidth, mTextureHeight);
            TextureManager.GetInstance().Draw(mTextureId, mPosition3, mTextureWidth, mTextureHeight);
            TextureManager.GetInstance().Draw(mTextureId, mPosition4, mTextureWidth, mTextureHeight);
            TextureManager.GetInstance().Draw(mTextureId, mPosition5, mTextureWidth, mTextureHeight);
        }

        public void OnResolutionChanged()
        {
            mTextureWidth = Globals.mGraphicsDevice.Viewport.Width;
            mTextureHeight = Globals.mGraphicsDevice.Viewport.Height;
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
