using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.InputManagement
{
    public enum ActionType
    {
        Exit,
        CameraZoomIn,
        CameraZoomOut,
        CameraZoomInFast,
        CameraZoomOutFast,
        MoveRight,
        MoveLeft,
        MoveUp,
        MoveDown,
        SaveGame,
        LoadGame,
        ToggleFullscreen,
        GoHome,
        Accelerate,
        Deaccelerate,
        MaxSpeed,
        MinSpeed,
        Test,
        Debug,
        ToggleRayTracing
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
