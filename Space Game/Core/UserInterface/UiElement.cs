using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Galaxy_Explovive.Core.UserInterface
{
    public abstract class UiElement
    {
        protected TextureManager mTextureManager;
        protected GraphicsDevice mGraphicsDevice;
        internal UiCanvas mCanvas;

        protected UiElement(UiFrame root, TextureManager textureManager, GraphicsDevice graphicsDevice)
        {
            root?.AddToChilds(this);
            mCanvas = new(root);
            mTextureManager = textureManager;
            mGraphicsDevice = graphicsDevice;
        }

        public abstract void Update(InputState inputState);
        public abstract void OnResolutionChanged();
        public abstract void Draw();

    }
}
