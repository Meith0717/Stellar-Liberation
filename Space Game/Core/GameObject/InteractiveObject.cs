using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;
using System;
using System.Runtime.CompilerServices;

namespace Galaxy_Explovive.Core.GameObject
{
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        // Selection Stuff
        public bool IsHover { get; set; }
        public string HoverTextureId { get; set; }
        public float HoverRadius { get; set; }
        public bool IsLeftClick { get; set; }
        public bool IsRightClick { get; set; }

        public void UpdateInputs(InputState inputState)
        {
            IsLeftClick = IsRightClick = false;

            var mousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            IsHover = Vector2.Distance(mousePosition, Position) < HoverRadius;

            if (!IsHover) { return; }
            if (inputState.mMouseActionType == MouseActionType.LeftClick) { IsLeftClick = true; }
            if (inputState.mMouseActionType == MouseActionType.RightClick) { IsRightClick = true; }
        }
    }
}
