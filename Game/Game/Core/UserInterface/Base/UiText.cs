// UiText.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiText(string fontID, string text, float scale) : UiElement
    {
        public string Text = text;
        public Color Color = new(192, 192, 192);
        public float Scale = scale;
        private readonly string mFontID = fontID;

        private static Vector2 GetTextDimension(string fontID, string text)
            => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        private Vector2 mTextDim;

        public override void Update(InputState inputState, GameTime gameTime)
        {
            mTextDim = GetTextDimension(mFontID, Text);
            Height = (int)(mTextDim.Y * Scale);
            Width = (int)(mTextDim.X * Scale);
            base.Update(inputState, gameTime);
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            mTextDim = GetTextDimension(mFontID, Text);
            Height = (int)(mTextDim.Y * Scale);
            Width = (int)(mTextDim.X * Scale);
            base.ApplyResolution(root, resolution);
        }

        public override void Draw()
        {
            TextureManager.Instance.DrawString(mFontID, Position, Text, mUiScale * Scale, Color);
            DrawCanvas();
        }
    }
}
