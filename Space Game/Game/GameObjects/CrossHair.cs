using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Game.Core.InputManagement;
using Space_Game.Core.TextureManagement;

namespace Space_Game.Core.GameObject
{
    public class CrossHair : GameObject
    {
        private float mMaxScale;
        private float mMinScale;

        private bool mHover;
        public bool mDrawInnerRing;

        public CrossHair(float minScale, float maxScale, Vector2 position)
        {
            // Location
            Position = position;
            Rotation = 0;

            // Rendering
            TextureWidth = 1024;
            TextureHeight = 1024;
            TextureOffset = new Vector2(TextureWidth, TextureHeight) / 2;
            TextureSclae = mMinScale = minScale;
            TextureDepth = 0;
            mMaxScale = maxScale;
            mDrawInnerRing = false;
        }
        public void Draw(Color color)
        {
            TextureManager.GetInstance().Draw("crossHair1", Position, TextureOffset, TextureWidth, TextureHeight,
                TextureSclae, 0, 1, color);
            if (!mDrawInnerRing) { return; }
            TextureManager.GetInstance().Draw("crossHair1", Position, TextureOffset, TextureWidth, TextureHeight,
                TextureSclae, Rotation, 1, color);
            mDrawInnerRing = false;
        }
        public void Update(Vector2 position, bool isHover)
        {
            mHover = isHover;
            Position = position;
            Rotation += 0.05f;
            Animation();
        }
        private void Animation()
        {
            if (mHover)
            {
                if (TextureSclae < mMaxScale)
                {
                    TextureSclae += 0.075f;
                }
            }
            else
            {
                if (TextureSclae > mMinScale)
                {
                    TextureSclae -= 0.075f;
                }
            }
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            throw new System.NotImplementedException();
        }
        public override void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}