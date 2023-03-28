using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public abstract class AstronomicalBody : SelectableObject
    {
        public override void UpdateInputs(InputState inputState)
        {
            if (!IsHover) { return; }
            if (inputState.mMouseActionType == MouseActionType.LeftClick)
            {
                Globals.mCamera2d.mTargetPosition = Position;
                Globals.mCamera2d.SetZoom(0.2f);
            }
        }
    }
}
