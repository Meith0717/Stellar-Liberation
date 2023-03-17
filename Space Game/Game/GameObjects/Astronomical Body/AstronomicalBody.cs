using Space_Game.Core;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;

namespace Space_Game.Game.GameObjects.Astronomical_Body
{
    public abstract class AstronomicalBody : SelectableObject
    {
        public override void UpdateInputs(InputState inputState)
        {
            if (!IsHover) { return; }
            if (inputState.mMouseActionType == MouseActionType.LeftClick)
            {
                Globals.mCamera2d.mTargetPosition = Position;
                Globals.mCamera2d.SetZoom(1);
            }
        }
    }
}
