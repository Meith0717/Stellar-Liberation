using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.GameEngine.Rendering;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace GalaxyExplovive.Core.UserInterface
{
    internal class MyUiSprite
    {
        private bool mHover;
        private float mX;
        private float mY;
        private string mTexture;
        public bool Disabled = false;
        public bool Hide = false;
        public Color Color = Color.White;
        public Color HoverColor = Globals.HoverColor;
        public Action OnClickAction = null;
        public float Scale = 1f;
        public MouseActionType MouseActionType = MouseActionType.LeftClick;

        public MyUiSprite(float x, float y, string texture)
        {
            mX = x;
            mY = y;
            mTexture = texture;
        }

        public void Update(TextureManager textureManager, InputState inputState)
        {
            if (Hide || Disabled) return;
            var texture = textureManager.GetTexture(mTexture);
            var rect = new RectangleF(mX, mY, texture.Width * Scale, texture.Height * Scale);
            mHover = rect.Contains(inputState.mMousePosition);
            if (mHover &&
                inputState.mMouseActionType == MouseActionType &&
                OnClickAction != null)
            { OnClickAction(); inputState.mMouseActionType = MouseActionType.None; }
        }

        public void OnResolutionChanged(float x, float y)
        {
            if (Hide) return;
            mX = x;
            mY = y;
        }

        public void Draw(TextureManager textureManager)
        {
            if (Hide) return;
            textureManager.Draw(mTexture, new(mX, mY), Vector2.Zero, Scale, 0, 1,
               Disabled ? Color.Transparent : (mHover ? HoverColor : Color));
        }


    }
}
