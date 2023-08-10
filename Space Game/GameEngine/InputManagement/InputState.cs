/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.GameEngine.InputManagement
{
    public enum ActionType
    {
        ESC,
        CameraZoomIn,
        CameraZoomOut,
        ToggleFullscreen,
        ToggleDebugModes,
        Acceleration,
        Deacceleration,
        FireSecondaryWeapon,
        FireInitialWeapon,
    }

    public enum MouseActionType
    {
        None,
        LeftClick,
        RightClick,
        LeftClickHold,
        RightClickHold,
        LeftClickReleased,
        MouseWheelForward,
        MouseWheelBackward
    }

    public enum KeyEventType
    {
        OnButtonDown,
        OnButtonPressed
    }

    internal class GamePadValues
    {
        internal Vector2 mLeftThumbSticks;
        internal Vector2 mRightThumbSticks;
        internal float mLeftTrigger;
        internal float mRightTrigger;
    }

    public class InputState
    {
        internal readonly List<ActionType> mActionList;
        internal MouseActionType mMouseActionType;
        internal Vector2 mMousePosition;
        internal GamePadValues mGamePadValues;
        internal GamePadValues mPrevGamePadValues;

        // Constructor.
        public InputState()
        {
            mActionList = new List<ActionType>();
        }
    }
}
