using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiLayer : UiElement
    {
        // Public Atributes
        public double BgColorAlpha { get; set; } = 1;
        public Color BgColor { get; set; } = Color.White;
        public int MinWidth { get; set; } = 0;
        public int MinHeight { get; set; } = 0;

        // Private Atributes
        private Color mLayerColor;
        private readonly List<UiElement> mChilds = new();

        public UiLayer(UiLayer root, double relX, double relY, double relWidth, double relHeight) 
        { 
            RelX = relX;
            RelY = relY;
            RelWidth = relWidth;
            RelHeight = relHeight;
            Root = root;
            GetRectangle();
        }

        public override void Update(InputState inputState)
        {
            mLayerColor = new Color(
                (int)(BgColor.R * BgColorAlpha), 
                (int)(BgColor.G * BgColorAlpha), 
                (int)(BgColor.B * BgColorAlpha), 
                (int)(255 * BgColorAlpha)
            );
            if (Rectangle.Width < MinWidth) { Rectangle.Width = MinWidth; }
            if (Rectangle.Height < MinHeight) { Rectangle.Width = MinHeight; }
            foreach (var child in mChilds) { child.Update(inputState); }
        }

        public override void Draw()
        {
            TextureManager textureManager = TextureManager.Instance;
            textureManager.GetSpriteBatch().Draw(textureManager.GetTexture("UiLayer"), Rectangle, mLayerColor);
            foreach (UiElement child in mChilds) { child.Draw(); }
        }

        public override void OnResolutionChanged()
        {
            GetRectangle();
            foreach (var child in mChilds) { child.OnResolutionChanged(); }
        }

        public void AddToChilds(UiElement child)
        {
            mChilds.Add(child);
        }

        private void GetRectangle()
        {
            Vector2 startPos = Vector2.Zero;
            int width = Globals.mGraphicsDevice.Viewport.Width;
            int height = Globals.mGraphicsDevice.Viewport.Height;

            if (Root != null)
            {
                startPos = new(Root.Rectangle.X, Root.Rectangle.Y);
                width = Root.Rectangle.Width;
                height = Root.Rectangle.Height;
            }

            float screenWidth = (float)(width * RelWidth);
            float screenHeight = (float)(height * RelHeight);

            float screenX = (float)(width * RelX) - (screenWidth / 2) + startPos.X;
            float screenY = (float)(height * RelY) - (screenHeight / 2) + startPos.Y;
            Rectangle = new RectangleF(screenX, screenY, screenWidth, screenHeight).ToRectangle();
        }
    }
}
