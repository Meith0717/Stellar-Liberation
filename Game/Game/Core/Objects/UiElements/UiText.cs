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

        public UiText(string fontID, string text)
        {
            Color = Color.White;

            Text = text;
            FontID = fontID;
        }

        public override void Initialize(Rectangle root, float UiScaling) => OnResolutionChanged(root, 1);

        public override void OnResolutionChanged(Rectangle root, float UiScaling)
        {
            mTextDim = GetTextDimension(FontID, Text);
            mCanvas.Height = (int)mTextDim.Y;
            mCanvas.Width = (int)mTextDim.X;
            mCanvas.UpdateFrame(root);
        }

        private Vector2 GetTextDimension(string fontID, string text) => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw()
        {
            var textPos = new Vector2(mCanvas.Bounds.Left, mCanvas.Center.Y - (mTextDim.Y / 2) + 3);
            TextureManager.Instance.DrawString(FontID, textPos, Text, 1, Color);
            mCanvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float UiScaling) => OnResolutionChanged(root, 1);
    }
}
