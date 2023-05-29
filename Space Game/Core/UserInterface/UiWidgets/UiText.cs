﻿using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiText : UiElement
    {
        public Color FontColor { get; set; } = Color.Black;
        public string Text { get; set; } = "UiText";
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;

        private string mFont = "text";

        public UiText(UiFrame root, TextureManager textureManager, GraphicsDevice graphicsDevice) 
            : base(root, textureManager, graphicsDevice)
        {
            Canvas = new(root);
        }

        public override void Draw()
        {
            Vector2 position = new(Canvas.ToRectangle().X, Canvas.ToRectangle().Y);
            mTextureManager.DrawString(mFont, position, Text, FontColor);
        }

        public override void OnResolutionChanged()
        {
        }

        public override void Update(InputState inputState)
        {
            Vector2 stringDimensions = mTextureManager.GetSpriteFont(mFont).MeasureString(Text);
            Canvas.Width = stringDimensions.X;
            Canvas.Height = stringDimensions.Y;
            Canvas.RelativeX = RelativX;
            Canvas.RelativeY = RelativY;
            Canvas.OnResolutionChanged(mGraphicsDevice);
        }
    }
}
