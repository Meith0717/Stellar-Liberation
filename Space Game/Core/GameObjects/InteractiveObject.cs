using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using Galaxy_Explovive.Game;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class InteractiveObject : GameObject
    {
        public InteractiveObject(GameLayer gameLayer) : base(gameLayer) {}

        protected bool IsHover { get; private set; }

        protected void UpdateInputs(InputState inputState)
        {
            BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
            var mousePosition = mGameLayer.mCamera.ViewToWorld(inputState.mMousePosition.ToVector2());
            IsHover = BoundedBox.Contains(mousePosition);
        }
    }
}
