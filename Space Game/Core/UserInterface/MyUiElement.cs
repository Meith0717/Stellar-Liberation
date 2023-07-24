

using CelestialOdyssey.Core.GameEngine.Content_Management;

namespace CelestialOdyssey.Core.UserInterface
{
    internal abstract class MyUiElement
    {
        public abstract void Update();
        public abstract void OnResolutionChanged();
        public abstract void Draw(TextureManager textureManager);
    }
}
