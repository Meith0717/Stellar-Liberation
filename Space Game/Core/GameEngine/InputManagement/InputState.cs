/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GalaxyExplovive.Core.GameEngine.InputManagement
{
    public enum ActionType
    {
        ESC,
        CameraZoomIn,
        CameraZoomOut,
        ToggleFullscreen,
        GoHome,
        Accelerate,
        Decelerate,
        Test,
        Debug,
        ToggleRayTracing,
        ToggleHUD,
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

    [Serializable]
    public class InputState
    {
        [JsonProperty] internal readonly List<ActionType> mActionList;
        [JsonProperty] internal MouseActionType mMouseActionType;
        [JsonProperty] internal Point mMousePosition;
        [JsonProperty] internal Rectangle mMouseRectangle;

        // Constructor.
        public InputState()
        {
            mActionList = new List<ActionType>();
        }
    }
}
