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
                mGameLayer.SelectObject = this;
                if (this.GetType() == typeof(Planet))
                {
                    mGameLayer.mCamera.SetZoom(1.2f);
                }
                if (this.GetType() == typeof(Star))
                {
                    mGameLayer.mCamera.SetZoom(0.25f);
                }
            }

            if (!mTrack) { return; }
            mGameLayer.mCamera.TargetPosition = Position;
            if (mGameLayer.mCamera.mIsMoving || 
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mGameLayer.SelectObject = null;
                mTrack = false;
            }
        }
    }
}
