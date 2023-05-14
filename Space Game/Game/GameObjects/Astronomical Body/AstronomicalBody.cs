﻿using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;

namespace Galaxy_Explovive.Game.GameObjects.Astronomical_Body
{
    public abstract class AstronomicalBody : InteractiveObject
    {
        private bool mTrack = false;

        public new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);
            if (inputState.mMouseActionType == MouseActionType.LeftClick && IsHover)
            {
                mTrack = true;
                if (this.GetType() == typeof(Planet))
                {
                    Globals.mCamera2d.SetZoom(1.2f);
                }
                if (this.GetType() == typeof(Star))
                {
                    Globals.mCamera2d.SetZoom(0.25f);
                }
            }

            if (!mTrack) { return; }
            Globals.mCamera2d.mTargetPosition = Position;
            if (Globals.mCamera2d.mIsMoving || 
                (inputState.mMouseActionType == MouseActionType.LeftClick && !IsHover))
            {
                mTrack = false;
            }
        }
    }
}
