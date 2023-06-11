using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.Layers;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public abstract class AstronomicalBody : InteractiveObject
    {
        private bool mTrack = false;

        protected AstronomicalBody(GameLayer gameLayer) : base(gameLayer) {}

        public override void UpdateInputs(InputState inputState)
        {
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                mTrack = true;
                if (GetType() == typeof(Planet))
                {
                    mGameLayer.mCamera.SetZoom(1.2f);
                }
                if (GetType() == typeof(Star))
                {
                    mGameLayer.mCamera.SetZoom(0.25f);
                }
            }

            if (mGameLayer.mCamera.mIsMoving || 
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
                mGameLayer.SelectObject = null;
            }
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
            if (!mTrack) { return; }
            mGameLayer.mCamera.TargetPosition = Position;
        }
    }
}
