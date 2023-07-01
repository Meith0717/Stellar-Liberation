using GalaxyExplovive.Core.GameEngine.Rendering;

namespace GalaxyExplovive.Core.UserInterface
{
    internal abstract class MyUiElement
    {
        public abstract void Update();
        public abstract void OnResolutionChanged();
        public abstract void Draw(TextureManager textureManager);
    }
}
