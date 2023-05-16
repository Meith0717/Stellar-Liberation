
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.UserInterface
{
    public abstract class UiElement
    {

        protected UiElement(UiLayer root)
        {
            root?.Addchild(this);
        }

        internal UiCanvas Canvas { get; set; }
        public abstract void Update(InputState inputState);
        public abstract void OnResolutionChanged();
        public abstract void Draw();

        
    }
}
