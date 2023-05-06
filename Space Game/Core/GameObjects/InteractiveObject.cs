using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class InteractiveObject : GameObject
    {
        public bool IsHover { get; private set; }

        public void UpdateInputs(InputState inputState)
        {
            BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureSclae);
            var mousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            IsHover = BoundedBox.Contains(mousePosition);
        }
    }
}
