using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;

namespace Galaxy_Explovive.Core.UserInterface.UiWidgets
{
    public abstract class UiElement
    {
        protected TextureManager mTextureManager;

        protected UiElement(UiFrame root, TextureManager textureManager)
        {
            root?.AddToChilds(this);
            Canvas = new(root);
            mTextureManager = textureManager;
        }

        internal UiCanvas Canvas { get; set; }
        public abstract void Update(InputState inputState);
        public abstract void OnResolutionChanged();
        public abstract void Draw();

    }
}
