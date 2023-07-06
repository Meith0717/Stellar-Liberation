
using GalaxyExplovive.Core.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GalaxyExplovive.Core.UserInterface
{
    internal class MyUiFrame
    {
        public RectangleF Frame;
        public Color Color = Color.White;
        public float Alpha = 1.0f;

        public MyUiFrame(float x, float y, float width, float height)
        {
            Frame = new(x, y, width, height);
        }

        public void OnResolutionChanged(float x, float y, float width, float height)
        {
            Frame.X = x;
            Frame.Y = y;
            Frame.Width = width;
            Frame.Height = height;
        }

        public void Draw(TextureManager textureManager)
        {
            Color c = new((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            textureManager.SpriteBatch.Draw(textureManager.GetTexture("Layer"), Frame.ToRectangle(), c);
        }
    }
}
