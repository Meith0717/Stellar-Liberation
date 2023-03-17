using Microsoft.Xna.Framework;
using Space_Game.Core.InputManagement;
using System;
using System.Runtime.CompilerServices;

namespace Space_Game.Core.GameObject
{
    [Serializable]
    public abstract class SelectableObject : GameObject
    {
        // Selection Stuff
        public bool IsHover { get; set; }
        public bool IsSelect { get; set; }
        public string HoverTextureId { get; set; }
        public float HoverRadius { get; set; }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            var mousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            IsHover = Vector2.Distance(mousePosition, Position) < HoverRadius;
        }

        public abstract void UpdateInputs(InputState inputState);
    }
}
