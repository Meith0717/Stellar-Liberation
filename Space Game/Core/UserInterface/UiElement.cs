
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.ComponentModel.DataAnnotations;

namespace Galaxy_Explovive.Core.UserInterface
{
    public abstract class UiElement
    {
        public double RelX { get; set; }
        public double RelY { get; set; }
        public double RelWidth { get; set; }
        public double RelHeight { get; set; }
        public UiLayer Root { get; set; }
        public Rectangle Rectangle;

        public abstract void Update(InputState inputState);
        public abstract void OnResolutionChanged();
        public abstract void Draw();
    }
}
