using CelestialOdyssey.Game.Core.InputManagement;
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

        public List<ActionType> GetAction(out MouseActionType actionType)
        {
            actionType = MouseActionType.None;
            mPreviousMouseState = mCurrentMouseState;

            // Update current and previous MouseWheelValue
            mPreviousMouseWheelValue = mCurrentMouseWheelValue;
            mCurrentMouseWheelValue = mCurrentMouseState.ScrollWheelValue;

            mCurrentMouseState = Mouse.GetState();

            if (mCurrentMouseState.LeftButton == ButtonState.Pressed)
                actionType = !IsLeftMouseButtonDown() ? MouseActionType.LeftClick : MouseActionType.LeftClickHold;
            if (mCurrentMouseState.RightButton == ButtonState.Pressed)
                actionType = !IsRightMouseButtonDown() ? MouseActionType.RightClick : MouseActionType.RightClickHold;
            if (IsLeftMouseButtonReleased())
                actionType = MouseActionType.LeftClickReleased;

            // Set Mouse Action to MouseWheel
            if (mCurrentMouseWheelValue > mPreviousMouseWheelValue)
                actionType = MouseActionType.MouseWheelForward;
            if (mCurrentMouseWheelValue < mPreviousMouseWheelValue)
                actionType = MouseActionType.MouseWheelBackward;

            // Add actions to InputState.mActionList based on MouseAction.
            List<ActionType> actions = new List<ActionType>();
            foreach (var key in mKeyBindingsMouse.Keys)
            {
                if (key == actionType)
                {
                    actions.Add(mKeyBindingsMouse[key]);
                }
            }
            return actions;
        }
    }
}
