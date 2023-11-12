// UiText.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using System;
using static System.Net.Mime.MediaTypeNames;

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
            base.Initialize(root);
            var dim = GetTextDimension(FontID, Text);
            Width = (int)dim.X;
            Height = (int)dim.Y;
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            base.OnResolutionChanged(root);
            var dim = GetTextDimension(FontID, Text);
            Width = (int)dim.X;
            Height = (int)dim.Y;
        }

        private Vector2 GetTextDimension(string fontID, string text) => TextureManager.Instance.GetFont(fontID).MeasureString(text);

        public override void Draw() => TextureManager.Instance.DrawString(FontID, Frame.Location.ToVector2(), Text, 1, Color);
    }
}
