﻿
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.UserInterface
{
    internal class MyUiText
    {
        float mX; 
        private float mY;
        private string mFont;
        public string Text;
        public Color Color = Color.White;

        public MyUiText(float x, float y, string text, string font = "text") 
        {
            mX = x;
            mY = y;
            Text = text;
            mFont = font;

        }

        public void OnResolutionChanged(float x, float y)
        {
            mX = x;
            mY = y;
        }

        public void Draw(TextureManager textureManager)
        {
            textureManager.DrawString(mFont, new(mX, mY), Text, 1, Color);
        }
    }
}