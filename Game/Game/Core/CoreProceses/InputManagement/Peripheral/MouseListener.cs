// MouseListener.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.InputManagement.Peripheral
{
    internal class MouseListener
    {
        private MouseState mCurrentState, mPreviousState;
        private int mCurrentMouseWheelValue, mPreviousMouseWheelValue;

        // Return true if mouse was constantly down.
        private bool LeftMouseButtonDown => mCurrentState.LeftButton == ButtonState.Pressed && mPreviousState.LeftButton == ButtonState.Pressed;
        private bool RightMouseButtonDown => mCurrentState.RightButton == ButtonState.Pressed && mPreviousState.RightButton == ButtonState.Pressed;
        private bool LeftMouseButtonReleased => mCurrentState.LeftButton == ButtonState.Released && mPreviousState.LeftButton == ButtonState.Pressed;
        private bool RightMouseButtonReleased => mCurrentState.RightButton == ButtonState.Released && mPreviousState.RightButton == ButtonState.Pressed;

        private readonly Dictionary<ActionType, ActionType> mKeyBindingsMouse;

        public MouseListener()
        {
            mKeyBindingsMouse = new()
            {
                { ActionType.MouseWheelBackward, ActionType.CameraZoomOut },
                { ActionType.MouseWheelForward, ActionType.CameraZoomIn },
                { ActionType.LeftClickReleased, ActionType.Select },
            };
        }
        public void Listen(ref List<ActionType> actions, out Vector2 mousePosition)
        {
            mPreviousState = mCurrentState;

            // Update current and previous MouseWheelValue
            mPreviousMouseWheelValue = mCurrentMouseWheelValue;
            mCurrentMouseWheelValue = mCurrentState.ScrollWheelValue;

            mCurrentState = Mouse.GetState();
            mousePosition = mCurrentState.Position.ToVector2();

            if (mCurrentState.LeftButton == ButtonState.Pressed) actions.Add(!LeftMouseButtonDown ? ActionType.LeftClick : ActionType.LeftClickHold);
            if (mCurrentState.RightButton == ButtonState.Pressed) actions.Add(!RightMouseButtonDown ? ActionType.RightClick : ActionType.RightClickHold);
            if (LeftMouseButtonReleased) actions.Add(ActionType.LeftClickReleased);
            if (RightMouseButtonReleased) actions.Add(ActionType.RightClickReleased);

            // Set Mouse Action to MouseWheel
            if (mCurrentMouseWheelValue > mPreviousMouseWheelValue)
                actions.Add(ActionType.MouseWheelForward);
            if (mCurrentMouseWheelValue < mPreviousMouseWheelValue)
                actions.Add(ActionType.MouseWheelBackward);

            foreach (var key in mKeyBindingsMouse.Keys)
            {
                if (actions.Contains(key)) actions.Add(mKeyBindingsMouse[key]);
            }
        }
    }
}
