// UiText.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
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

        private Vector2 GetTextDimension(string fontID, string text) => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw()
        {
            TextureManager.Instance.DrawString(FontID, Canvas.Position, Text, mUiScaling, Color);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            mUiScaling = uiScaling;
            mTextDim = GetTextDimension(FontID, Text);
            Height = (int)mTextDim.Y;
            Width = (int)mTextDim.X;
            Canvas.UpdateFrame(root, uiScaling);
        }
    }
}
