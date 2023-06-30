using Galaxy_Explovive.Core.GameEngine.Rendering;

namespace Galaxy_Explovive.Core.UserInterface
{
    internal abstract class MyUiElement
    {
        public abstract void Update();
        public abstract void OnResolutionChanged();
        public abstract void Draw(TextureManager textureManager);
    }
}
