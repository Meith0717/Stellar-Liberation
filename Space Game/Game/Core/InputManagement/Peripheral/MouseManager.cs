// MouseManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.InputManagement.Peripheral
{
    internal class MouseManager
    {
        private MouseState mCurrentMouseState, mPreviousMouseState;
        private int mCurrentMouseWheelValue, mPreviousMouseWheelValue;
        private readonly Dictionary<MouseActionType, ActionType> mKeyBindingsMouse;

        public MouseManager()
        {
            mKeyBindingsMouse = new()
            {
                { MouseActionType.MouseWheelBackward, ActionType.CameraZoomOut },
                { MouseActionType.MouseWheelForward, ActionType.CameraZoomIn },
            };
        }

        // Return true if mouse was constantly down.
        private bool IsLeftMouseButtonDown()
        {
            return mCurrentMouseState.LeftButton == ButtonState.Pressed &&
                    mPreviousMouseState.LeftButton == ButtonState.Pressed;
        }

        private bool IsRightMouseButtonDown()
        {
            return mCurrentMouseState.RightButton == ButtonState.Pressed &&
                    mPreviousMouseState.RightButton == ButtonState.Pressed;
        }

        // Is mouse button released?
        private bool IsLeftMouseButtonReleased()
        {
            return mCurrentMouseState.LeftButton == ButtonState.Released &&
                    mPreviousMouseState.LeftButton == ButtonState.Pressed;
        }

        public Vector2 GetPosition()
        {
            return Mouse.GetState().Position.ToVector2();
        }

        public List<ActionType> GetAction(out List<MouseActionType> actionTypes)
        {
            actionTypes = new List<MouseActionType>();
            mPreviousMouseState = mCurrentMouseState;

            // Update current and previous MouseWheelValue
            mPreviousMouseWheelValue = mCurrentMouseWheelValue;
            mCurrentMouseWheelValue = mCurrentMouseState.ScrollWheelValue;

            mCurrentMouseState = Mouse.GetState();

            if (mCurrentMouseState.LeftButton == ButtonState.Pressed)
                actionTypes.Add(!IsLeftMouseButtonDown() ? MouseActionType.LeftClick : MouseActionType.LeftClickHold);
            if (mCurrentMouseState.RightButton == ButtonState.Pressed)
                actionTypes.Add(!IsRightMouseButtonDown() ? MouseActionType.RightClick : MouseActionType.RightClickHold);
            if (IsLeftMouseButtonReleased())
                actionTypes.Add(MouseActionType.LeftClickReleased);

            // Set Mouse Action to MouseWheel
            if (mCurrentMouseWheelValue > mPreviousMouseWheelValue)
                actionTypes.Add(MouseActionType.MouseWheelForward);
            if (mCurrentMouseWheelValue < mPreviousMouseWheelValue)
                actionTypes.Add(MouseActionType.MouseWheelBackward);

            // Add actions to InputState.mActionList based on MouseAction.
            List<ActionType> actions = new List<ActionType>();
            foreach (var key in mKeyBindingsMouse.Keys)
            {
                if (actionTypes.Contains(key))
                {
                    actions.Add(mKeyBindingsMouse[key]);
                }
            }
            return actions;
        }
    }
}
