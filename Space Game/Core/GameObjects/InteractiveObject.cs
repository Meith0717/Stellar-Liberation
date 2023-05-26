using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Game;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class InteractiveObject : GameObject
    {
        public InteractiveObject(GameLayer gameLayer) : base(gameLayer) {}

        public bool IsHover { get; private set; }

        public void UpdateInputs(InputState inputState)
        {
            BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
            var mousePosition = Globals.Camera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            IsHover = BoundedBox.Contains(mousePosition);
        }
    }
}
