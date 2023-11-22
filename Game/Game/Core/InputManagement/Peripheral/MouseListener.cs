// MouseManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.InputManagement.Peripheral
{
    internal class MouseListener
    {
        private MouseState mCurrentState, mPreviousState;
        private int mCurrentMouseWheelValue, mPreviousMouseWheelValue;
        private readonly Dictionary<MouseActionType, ActionType> mKeyBindingsMouse;

        public MouseListener()
        {
            mKeyBindingsMouse = new()
            {
                { MouseActionType.MouseWheelBackward, ActionType.CameraZoomOut },
                { MouseActionType.MouseWheelForward, ActionType.CameraZoomIn },
            };
        }

        // Return true if mouse was constantly down.
        private bool LeftMouseButtonDown => mCurrentState.LeftButton == ButtonState.Pressed && mPreviousState.LeftButton == ButtonState.Pressed;
        private bool RightMouseButtonDown =>  mCurrentState.RightButton == ButtonState.Pressed && mPreviousState.RightButton == ButtonState.Pressed;
        private bool LeftMouseButtonReleased => mCurrentState.LeftButton == ButtonState.Released && mPreviousState.LeftButton == ButtonState.Pressed;
        private bool RightMouseButtonReleased => mCurrentState.RightButton == ButtonState.Released && mPreviousState.RightButton == ButtonState.Pressed;


        public void Listen(ref List<ActionType> actions, out List<MouseActionType> actionTypes, out Vector2 mousePosition)
        {
            actionTypes = new();
            mPreviousState = mCurrentState;

            // Update current and previous MouseWheelValue
            mPreviousMouseWheelValue = mCurrentMouseWheelValue;
            mCurrentMouseWheelValue = mCurrentState.ScrollWheelValue;

            mCurrentState = Mouse.GetState();
            mousePosition = mCurrentState.Position.ToVector2();

            if (mCurrentState.LeftButton == ButtonState.Pressed) actionTypes.Add(!LeftMouseButtonDown ? MouseActionType.LeftClick : MouseActionType.LeftClickHold);
            if (mCurrentState.RightButton == ButtonState.Pressed) actionTypes.Add(!RightMouseButtonDown ? MouseActionType.RightClick : MouseActionType.RightClickHold);
            if (LeftMouseButtonReleased) actionTypes.Add(MouseActionType.LeftClickReleased);
            if (RightMouseButtonReleased) actionTypes.Add(MouseActionType.RightClickReleased);

            // Set Mouse Action to MouseWheel
            if (mCurrentMouseWheelValue > mPreviousMouseWheelValue) actionTypes.Add(MouseActionType.MouseWheelForward);
            if (mCurrentMouseWheelValue < mPreviousMouseWheelValue) actionTypes.Add(MouseActionType.MouseWheelBackward);

            // Add actions to InputState.mActionList based on MouseAction.
            foreach (var key in mKeyBindingsMouse.Keys)
            {
                if (actionTypes.Contains(key)) actions.Add(mKeyBindingsMouse[key]);
            }
        }
    }
}
