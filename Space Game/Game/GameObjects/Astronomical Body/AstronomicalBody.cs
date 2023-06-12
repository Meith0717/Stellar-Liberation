using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public abstract class AstronomicalBody : InteractiveObject
    {
        private bool mTrack = false;

        protected AstronomicalBody(Game game) : base(game) {}

        public override void SelectActions(InputState inputState)
        {
            if (IsPressed)
            {
                if (GetType() == typeof(Planet))
                {
                    mGame.mCamera.SetZoom(1.2f);
                }
                if (GetType() == typeof(Star))
                {
                    mGame.mCamera.SetZoom(0.25f);
                }
            }
            mGame.mCamera.TargetPosition = Position;

            if (mGame.mCamera.MovedByUser || (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mGame.SelectObject = null;
            }
        }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
        }
    }
}
