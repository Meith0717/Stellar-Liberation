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
        MoveUp,
        MoveDown,
        MoveL,
        MoveR,
        ToggleFullscreen,
        Test,
        ToggleDebugModes,
        ToggleHeadUpDisplay,
        ToggleSectorGrid,
        Stop
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

    public class InputState
    {
        internal readonly List<ActionType> mActionList;
        internal MouseActionType mMouseActionType;
        internal Point mMousePosition;


        // Constructor.
        public InputState()
        {
            mActionList = new List<ActionType>();
        }
    }
}
