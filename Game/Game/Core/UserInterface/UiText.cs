// UiText.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiText : UiElement
    {

        public string Text;
        public string FontID;
        public Color Color;

        public UiText(string fontID, string text) 
        {
            Color = Color.White;

            Text = text;
            FontID = fontID;
        }

        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            var dim = GetTextDimension(FontID, Text);
            mCanvas.Width = (int)dim.X;
            mCanvas.Height = (int)dim.Y;
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            var dim = GetTextDimension(FontID, Text);
            mCanvas.Width = (int)dim.X;
            mCanvas.Height = (int)dim.Y;
        }

        private Vector2 GetTextDimension(string fontID, string text) => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw() => TextureManager.Instance.DrawString(FontID, mCanvas.Center, Text, 1, Color);

        public override void Update(InputState inputState, Rectangle root){}
    }
}
