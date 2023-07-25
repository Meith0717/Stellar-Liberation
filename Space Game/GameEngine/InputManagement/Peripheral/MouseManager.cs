using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace CelestialOdyssey.GameEngine.InputManagement.Peripheral
{
    internal class MouseManager
    {
        private MouseState mCurrentMouseState, mPreviousMouseState;
        private int mCurrentMouseWheelValue, mPreviousMouseWheelValue;

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

        public MouseActionType GetAction()
        {
            mPreviousMouseState = mCurrentMouseState;

            // Update current and previous MouseWheelValue
            mPreviousMouseWheelValue = mCurrentMouseWheelValue;
            mCurrentMouseWheelValue = mCurrentMouseState.ScrollWheelValue;

            mCurrentMouseState = Mouse.GetState();

            if (mCurrentMouseState.LeftButton == ButtonState.Pressed)
                return !IsLeftMouseButtonDown() ? MouseActionType.LeftClick : MouseActionType.LeftClickHold;
            if (mCurrentMouseState.RightButton == ButtonState.Pressed)
                return !IsRightMouseButtonDown() ? MouseActionType.RightClick : MouseActionType.RightClickHold;
            if (IsLeftMouseButtonReleased())
                return MouseActionType.LeftClickReleased;

            // Set Mouse Action to MouseWheel
            if (mCurrentMouseWheelValue > mPreviousMouseWheelValue)
                return MouseActionType.MouseWheelForward;
            if (mCurrentMouseWheelValue < mPreviousMouseWheelValue)
                return MouseActionType.MouseWheelBackward;

            return MouseActionType.None;
        }
    }
}
