using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;
using System;
using System.Reflection;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.GameObject
{
    [Serializable]
    public abstract class InteractiveObject : GameObject
    {
        const string CrossHairNullError = "Please initialise the Crosshair in the InteractiveObject subclass.";
        const string TextureRadiusZeroError = "Please initialise the HoverRadius in the InteractiveObject subclass.";
        // Selection Stuff
        public bool IsHover { get; private set; }
        public CrossHair Crosshair { get; set; }
        public float TextureRadius { get; set; }

        public void UpdateInputs(InputState inputState)
        {
            if (Crosshair == null) { throw new Exception(CrossHairNullError); }
            if (TextureRadius == 0) { throw new Exception(TextureRadiusZeroError); }

            var mousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            IsHover = Vector2.Distance(mousePosition, Position) < TextureRadius;

            Crosshair.Update(Position, IsHover);
            BoundedBox = new CircleF(Position, TextureRadius);
        }
    }
}
