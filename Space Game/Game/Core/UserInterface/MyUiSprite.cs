using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace CelestialOdyssey.Game.Core.UserInterface
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
        public Color HoverColor = Color.Gray;
        public Action OnClickAction = null;
        public float Scale = 1f;
        public MouseActionType MouseActionType = MouseActionType.LeftClick;

        public MyUiSprite(float x, float y, string texture)
        {
            mX = x;
            mY = y;
            mTexture = texture;
        }

        public void Update(InputState inputState)
        {
            if (Hide || Disabled) return;
            var texture = TextureManager.Instance.GetTexture(mTexture);
            var rect = new RectangleF(mX, mY, texture.Width * Scale, texture.Height * Scale);
            mHover = rect.Contains(inputState.mMousePosition);
            if (mHover &&
                inputState.HasMouseAction(MouseActionType) &&
                OnClickAction != null)
            {
                OnClickAction();
            }
        }

        public void OnResolutionChanged(float x, float y)
        {
            if (Hide) return;
            mX = x;
            mY = y;
        }

        public void Draw()
        {
            if (Hide) return;
            TextureManager.Instance.Draw(mTexture, new(mX, mY), Vector2.Zero, Scale, 0, 1,
               Disabled ? Color.Transparent : mHover ? HoverColor : Color);
        }
    }
}
