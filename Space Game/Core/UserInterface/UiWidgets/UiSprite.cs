using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using static Galaxy_Explovive.Core.UserInterface.UiWidgets.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiSprite : UiElement
    {
        public RootFill Fill = RootFill.Fix;
        public float Scale = 1f;
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;

        private Texture2D mTexture;

        public UiSprite(UiFrame root, TextureManager textureManager, GraphicsDevice graphicsDevice, string texture) 
            : base(root, textureManager, graphicsDevice) 
        {
            mTexture = mTextureManager.GetTexture(texture);
        }

        public override void Draw()
        {
            mTextureManager.SpriteBatch.Draw(mTexture, mCanvas.ToRectangle(), Color.White);
        }

        public override void OnResolutionChanged()
        {
            Rectangle rootRectangle = mCanvas.GetRootRectangle(mGraphicsDevice).ToRectangle();
            mCanvas.RelativeX = RelativX;
            mCanvas.RelativeY = RelativY;
            mCanvas.Width = mTexture.Width;
            mCanvas.Height = mTexture.Height;
            mCanvas.Fill = Fill;
            mCanvas.OnResolutionChanged(mGraphicsDevice);
        }

        public override void Update(InputState inputState)
        {
        }
    }
}


