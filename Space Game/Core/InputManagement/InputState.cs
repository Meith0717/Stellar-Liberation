using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace rache_der_reti.Core.InputManagement
{
    public enum ActionType
    {
        Exit,
        CameraRight,
        CameraLeft,
        CameraUp,
        CameraDown,
        CameraZoomIn,
        CameraZoomOut,
        CameraZoomInFast,
        CameraZoomOutFast,
        MoveRight,
        MoveLeft,
        MoveUp,
        SaveGame,
        LoadGame,
        ToggleFullscreen,
        DoSomething
    }

    public enum MouseActionType
    {
        None,
        LeftClick,
        RightClick,
        LeftClickDouble,
        RightClickDouble,
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
