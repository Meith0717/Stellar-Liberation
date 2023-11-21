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
        MouseWheelForward,
        MouseWheelBackward
    }

    public enum KeyEventType
    {
        OnButtonDown,
        OnButtonPressed
    }

    public struct GamePadValues
    {
        internal Vector2 mLeftThumbSticks = Vector2.Zero;
        internal Vector2 mRightThumbSticks = Vector2.Zero;
        internal float mLeftTrigger = new();
        internal float mRightTrigger = new();

        public GamePadValues() {; }
    }

    public struct InputState
    {
        public List<ActionType> mActions = new();
        public List<MouseActionType> mMouseActions = new();
        public Vector2 mMousePosition = Vector2.Zero;
        public GamePadValues mGamePadValues = new();

        public InputState(List<ActionType> actions, List<MouseActionType> mouseActions, Vector2 mousePosition, GamePadValues gamePadValues)
        {
            mActions = actions;
            mMouseActions = mouseActions;
            mMousePosition = mousePosition;
            mGamePadValues = gamePadValues;
        }

        public bool HasMouseAction(MouseActionType mouseActionType) => mMouseActions.Remove(mouseActionType);

        public void DoMouseAction(MouseActionType mouseActionType, Action funktion)
        {
            if (HasMouseAction(mouseActionType)) funktion();
        }

        public bool HasAction(ActionType action) => mActions.Remove(action);

        public void DoAction(ActionType action, Action funktion)
        {
            if (HasAction(action)) funktion();
        }
    }
}
