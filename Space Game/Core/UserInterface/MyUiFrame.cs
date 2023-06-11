using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.UserInterface
{
    internal class MyUiFrame
    {
        public bool Hide = false;  
        public RectangleF Frame;
        public Color Color = Color.White;
        public float Alpha = 1.0f;

        public MyUiFrame(float centerX, float centerY, float width, float height)
        {
            Frame = new(centerX - width / 2, centerY - height / 2, width, height);
        }

        public void OnResolutionChanged(float centerX, float centerY, float width, float height)
        { 
            if (Hide) return;
            Frame.X = centerX - width / 2;
            Frame.Y = centerY - height / 2;
            Frame.Width = width;
            Frame.Height = height;
        }

        public void Update()
        {
            if (Hide) return;
        }

        public void Draw(TextureManager textureManager)
        {
            if (Hide) return;
            Color c = new((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            textureManager.SpriteBatch.Draw(textureManager.GetTexture("Layer"), Frame.ToRectangle(), c);
        }
    }
}
