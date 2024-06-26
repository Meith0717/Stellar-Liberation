// UiText.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiText : UiElement
    {
        public string Text;
        public string FontID;
        public Color Color;
        private Vector2 mTextDim;
        public float Scale;

        public UiText(string fontID, string text, float scale)
        {
            Color = new(192, 192, 192);
            Text = text;
            FontID = fontID;
            Scale = scale;
        }

        private static Vector2 GetTextDimension(string fontID, string text)
            => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw()
        {
            TextureManager.Instance.DrawString(FontID, Canvas.Position, Text, mUiScale * Scale, Color);
            Canvas.Draw();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            base.ApplyResolution(root, resolution);
            mTextDim = GetTextDimension(FontID, Text);
            Height = (int)(mTextDim.Y * Scale);
            Width = (int)(mTextDim.X * Scale);
            Canvas.UpdateFrame(mRoot, mUiScale);
        }

        public override void Update(InputState inputState, GameTime gameTime)
        {
            mTextDim = GetTextDimension(FontID, Text);
            Height = (int)(mTextDim.Y * Scale);
            Width = (int)(mTextDim.X * Scale);
            Canvas.UpdateFrame(mRoot, mUiScale);
        }
    }
}
