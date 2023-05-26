using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.SoundManagement;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public abstract class AstronomicalBody : InteractiveObject
    {
        private bool mTrack = false;

        protected AstronomicalBody(GameLayer gameLayer) : base(gameLayer) {}

        public new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                mTrack = true;
                if (this.GetType() == typeof(Planet))
                {
                    Globals.Camera2d.SetZoom(1.2f);
                }
                if (this.GetType() == typeof(Star))
                {
                    Globals.Camera2d.SetZoom(0.25f);
                }
            }

            if (!mTrack) { return; }
            Globals.Camera2d.mTargetPosition = Position;
            if (Globals.Camera2d.mIsMoving || 
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
            }
        }
    }
}
