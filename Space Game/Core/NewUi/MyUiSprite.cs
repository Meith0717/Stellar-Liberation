using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Galaxy_Explovive.Core.NewUi
{
    internal class MyUiSprite
    {
        private bool mHover; 
        private float mX;
        private float mY;
        private string mTexture;
        public Color Color = Color.White;
        public Action OnClickAction = null;
        public float Scale = 1f;

        public MyUiSprite(float x, float y, string texture) 
        {
            mX = x;
            mY = y;
            mTexture = texture;
        }

        public void Update(TextureManager textureManager, InputState inputState)
        {
            var texture = textureManager.GetTexture(mTexture);
            var rect = new RectangleF(mX, mY, texture.Width, texture.Height);
            mHover = rect.Contains(inputState.mMousePosition);
            if (mHover && 
                inputState.mMouseActionType == MouseActionType.LeftClick && 
                OnClickAction != null) { OnClickAction(); }
        }

        public void OnResolutionChanged(float x, float y)
        {
            mX = x;
            mY = y;
        }

        public void Draw(TextureManager textureManager)
        {
            textureManager.Draw(mTexture, new(mX, mY), Vector2.Zero, Scale, 0, 1, mHover ? Color.Gray : Color);
        }


    }
}
