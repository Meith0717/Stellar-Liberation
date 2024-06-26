﻿// InputState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.InputManagement
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
        Space,
        ToggleHyperMap,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10,
        Accelerate,
        Break,
        CtrlLeft,
        Inventar,
        Trading,
        IncreaseScaling,
        DecreaseScaling,
        MoveCameraLeft,
        MoveCameraRight,
        MoveCameraUp,
        MoveCameraDown,
        ToggleCameraMode,
        UiDebug,
        BenchmarkPause,

        // Mouse
        LeftReleased,
        RightReleased,
        LeftClickHold,
        RightClickHold,
        LeftWasClicked,
        RightWasClicked,
        MouseWheelForward,
        MouseWheelBackward,
        MoveButtonUp,
        MoveButtonDown,
    }

    public enum GamePadActionType
    {
        None,
        LeftThumbStickUp,
        LeftThumbStickDown,
        RightThumbStickUp,
        RightThumbStickDown,
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
        public Vector2 mMousePosition = Vector2.Zero;
        public ThumbSticksState mThumbSticksState = new();
        public bool GamePadIsConnected = false;

        public InputState(List<ActionType> actions, Vector2 mousePosition, bool gamePadIsConnected, ThumbSticksState thumbSticksState)
        {
            Actions = actions;
            mMousePosition = mousePosition;
            mThumbSticksState = thumbSticksState;
            GamePadIsConnected = gamePadIsConnected;
        }

        public readonly bool HasAction(ActionType action) => Actions.Remove(action);

        public readonly void DoAction(ActionType action, Action funktion)
        {
            if (funktion is null) return;
            if (HasAction(action)) funktion();
        }
    }
}
