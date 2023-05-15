
using Galaxy_Explovive.Core.InputManagement;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.UserInterface
{
    public abstract class UiElement
    {
        internal UiCanvas Canvas { get; set; }
        public abstract void Update(InputState inputState);
        public abstract void OnResolutionChanged();
        public abstract void Draw();

        
    }
}
