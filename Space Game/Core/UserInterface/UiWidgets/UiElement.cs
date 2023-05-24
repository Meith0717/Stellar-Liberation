using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;

namespace Galaxy_Explovive.Core.UserInterface.UiWidgets
{
    public abstract class UiElement
    {
        protected UiElement(UiLayer root)
        {
            root?.AddToChilds(this);
            Canvas = new(root);
        }

        internal UiCanvas Canvas { get; set; }
        public abstract void Update(InputState inputState);
        public abstract void OnResolutionChanged();
        public abstract void Draw();

    }
}
