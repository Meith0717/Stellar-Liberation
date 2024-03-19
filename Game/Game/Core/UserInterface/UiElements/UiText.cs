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
        private float mUiScaling;

        public UiText(string fontID, string text)
        {
            Color = new(192, 192, 192);
            Text = text;
            FontID = fontID;
        }

        private static Vector2 GetTextDimension(string fontID, string text) 
            => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw()
        {
            TextureManager.Instance.DrawString(FontID, Canvas.Position, Text, mUiScaling, Color);
            Canvas.Draw();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            mTextDim = GetTextDimension(FontID, Text);
            Height = (int)mTextDim.Y;
            Width = (int)mTextDim.X;
            Canvas.UpdateFrame(root, mUiScaling = resolution.uiScaling);
        }

        public override void Update(InputState inputState, GameTime gameTime)
        {; }
    }
}
