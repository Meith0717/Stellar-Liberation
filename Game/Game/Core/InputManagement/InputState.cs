// InputState.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.InputManagement
{
    public enum ActionType
    {
        None,
        ESC,
        CameraZoomIn,
        CameraZoomOut,
        ToggleFullscreen,
        ToggleDebug,
        Load,
        Save,
        FireSecondaryWeapon,
        FireInitialWeapon,
        ToggleHyperMap,
        LeftClick,
        RightClick,
        LeftClickHold,
        RightClickHold,
        LeftClickReleased,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10,
        Accelerate,
        Break,
    }

    public enum MouseActionType
    {
        None,
        LeftClick,
        RightClick,
        LeftClickHold,
        RightClickHold,
        LeftClickReleased,
        RightClickReleased,
        MouseWheelForward,
        MouseWheelBackward
    }

    public enum GamePadActionType
    {
        None,
        LeftThumbStickUp,
        LeftThumbStickDown,
        RightThumbStickUp,
        RightThumbStickDown,
        Select
    }

    public enum KeyEventType
    {
        OnButtonDown,
        OnButtonPressed
    }

    public struct ThumbSticksState
    {
        public Vector2 LeftThumbSticks = Vector2.Zero;
        public Vector2 RightThumbSticks = Vector2.Zero;
        public float LeftTrigger = new();
        public float RightTrigger = new();

        public ThumbSticksState() {; }
    }

    public struct InputState
    {
        public List<ActionType> Actions = new();
        public List<MouseActionType> MouseActions = new();
        public List<GamePadActionType> GamePadActions = new();
        public Vector2 mMousePosition = Vector2.Zero;
        public ThumbSticksState mThumbSticksState = new();
        public bool GamePadIsConnected = false;

        public InputState(List<ActionType> actions, List<MouseActionType> mouseActions, List<GamePadActionType> gamePadActions, Vector2 mousePosition, bool gamePadIsConnected, ThumbSticksState thumbSticksState)
        {
            Actions = actions;
            MouseActions = mouseActions;
            mMousePosition = mousePosition;
            mThumbSticksState = thumbSticksState;
            GamePadIsConnected = gamePadIsConnected;
            GamePadActions = gamePadActions;
        }

        public bool HasMouseAction(MouseActionType mouseActionType) => MouseActions.Remove(mouseActionType);

        public void DoMouseAction(MouseActionType mouseActionType, Action funktion)
        {
            if (HasMouseAction(mouseActionType)) funktion();
        }

        public bool HasAction(ActionType action) => Actions.Remove(action);

        public void DoAction(ActionType action, Action funktion)
        {
            if (HasAction(action)) funktion();
        }
    }
}
