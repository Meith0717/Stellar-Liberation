// UiText.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiText : UiElement
    {

        public string Text;
        public string FontID;
        public Color Color;
        private Vector2 mTextDim;
        private float mUiScaling;

        public UiText(string fontID, string text)
        {
            Color = Color.White;

            Text = text;
            FontID = fontID;
        }

        public override void Initialize(Rectangle root, float UiScaling) => OnResolutionChanged(root, UiScaling);

        public override void OnResolutionChanged(Rectangle root, float UiScaling)
        {
            mUiScaling = UiScaling;
            mTextDim = GetTextDimension(FontID, Text);
            Height = (int)(mTextDim.Y * UiScaling);
            Width = (int)(mTextDim.X * UiScaling);
            Canvas.UpdateFrame(root, UiScaling);
        }

        private Vector2 GetTextDimension(string fontID, string text) => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw()
        {
            var textPos = new Vector2(Canvas.Bounds.Left, Canvas.Center.Y - (mTextDim.Y * mUiScaling / 2));
            TextureManager.Instance.DrawString(FontID, textPos, Text, mUiScaling ,Color);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float UiScaling) => OnResolutionChanged(root, UiScaling);
    }
}
