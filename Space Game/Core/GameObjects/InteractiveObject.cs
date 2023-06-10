using Galaxy_Explovive.Core.InputManagement;
using System;
using MonoGame.Extended;
using Galaxy_Explovive.Game;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class InteractiveObject : GameObject
    {
        public InteractiveObject(GameLayer gameLayer) : base(gameLayer) {}

        public bool IsHover { get; private set; }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            void CheckForHover(InputState inputState)
            {
                BoundedBox = new CircleF(Position, (Math.Max(TextureHeight, TextureWidth) / 2) * TextureScale);
                var mousePosition = mGameLayer.mCamera.ViewToWorld(inputState.mMousePosition.ToVector2());
                IsHover = BoundedBox.Contains(mousePosition);
            }

            CheckForHover(inputState);
        }

        public abstract void UpdateInputs(InputState inputState);
    }
}
