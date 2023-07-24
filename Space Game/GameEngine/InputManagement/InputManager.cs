/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.GameEngine.InputManagement
{
    public class InputManager
    {
        // Dictionary which contains actions for key inputs.
        private readonly Dictionary<Keys, ActionType> mKeyBindingsKeyboardPressed, mKeyBindingsKeyboardHold;

        // Attributes keyboard. new.
        private Keys[] mCurrentKeysPressed, mPreviousKeysPressed;

        // private KeyEvent mKeyEvent;
        private readonly Dictionary<Keys, KeyEventType> mKeysKeyEventTypes;

        // Attributes mouse.
        private MouseState mCurrentMouseState, mPreviousMouseState;
        private Point mCurrentMousePosition;

        private int mCurrentMouseWheelValue, mPreviousMouseWheelValue;

        // InputState contains all the actions made by the player and mouse position.
        private readonly InputState mInputState;

        // Constructor.
        public InputManager()
        {
            // Dictionary for keyboard keys that have been pressed and corresponding actions.
            mKeyBindingsKeyboardPressed = new()
            {
                { Keys.Escape, ActionType.ESC },
                { Keys.F11, ActionType.ToggleFullscreen},
                { Keys.F12, ActionType.ToggleDebugModes },
            };

            // Dictionary for keyboard keys that have been hold and corresponding actions.
            mKeyBindingsKeyboardHold = new Dictionary<Keys, ActionType>
            {
                { Keys.Left, ActionType.MoveL },
                { Keys.Right, ActionType.MoveR },
                { Keys.Up, ActionType.MoveUp },
                { Keys.Down, ActionType.MoveDown },
            };

            mInputState = new InputState();
            mKeysKeyEventTypes = new Dictionary<Keys, KeyEventType>();
        }

        private void UpdateMouseState()
        {
            // Get mouse state and position.
            mCurrentMouseState = Mouse.GetState();
            mCurrentMousePosition = Mouse.GetState().Position;

            // Add mouse position to InputState.
            mInputState.mMousePosition = mCurrentMousePosition;

            // Add mouse action for left and right button to InputState.
            if (RightMouseButtonPressed())
            {
                mInputState.mMouseActionType = !IsRightMouseButtonDown() ? MouseActionType.RightClick : MouseActionType.RightClickHold;
            }

            // Set Mouse Action to MouseWheel
            if (mCurrentMouseWheelValue > mPreviousMouseWheelValue)
            {
                mInputState.mMouseActionType = MouseActionType.MouseWheelForward;
            }
            if (mCurrentMouseWheelValue < mPreviousMouseWheelValue)
            {
                mInputState.mMouseActionType = MouseActionType.MouseWheelBackward;
            }
        }

        private void UpdateKeysKeyEventTypes()
        {
            // First clear Dictionary with Keys and KeyEventTypes.
            /*mKeysKeyEventTypes.Clear();*/

            // Get current keys pressed.
            mCurrentKeysPressed = Keyboard.GetState().GetPressedKeys();

            // Get KeyEventTypes (down or pressed) for keys.
            foreach (var key in mCurrentKeysPressed)
            {

                // Key is constantly pressed (down).
                if (mPreviousKeysPressed == null) { continue; }
                if (mPreviousKeysPressed.Contains(key))
                {
                    mKeysKeyEventTypes.Add(key, KeyEventType.OnButtonPressed);
                }
                // Key is pressed now (pressed).
                if (!mPreviousKeysPressed.Contains(key))
                {
                    mKeysKeyEventTypes.Add(key, KeyEventType.OnButtonDown);
                }
            }
        }

        private void UpdateKeyState()
        {
            // Get current keys pressed.
            mCurrentKeysPressed = Keyboard.GetState().GetPressedKeys();

            // Get current KeyEventTypes for keys pressed.
            UpdateKeysKeyEventTypes();


            // Add actions to InputState.mActionList depending on keys and KeyEventType.
            foreach (var key in mCurrentKeysPressed)
            {
                // Add actions to InputState.mActionList for keys down.
                if (mKeyBindingsKeyboardPressed.TryGetValue(key, out var actionPressed))
                {
                    if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonDown)
                    {
                        mInputState.mActionList.Add(actionPressed);
                    }
                }
                // Add actions to InputState.mActionList for keys pressed.
                if (!mKeyBindingsKeyboardHold.TryGetValue(key, out var actionHold)) return;
                if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonPressed)
                {
                    mInputState.mActionList.Add(actionHold);
                }
            }
        }

        private void SavePreviousKeyState()
        {
            mPreviousKeysPressed = mCurrentKeysPressed;
        }

        private void SavePreviousMouseState()
        {
            mPreviousMouseState = mCurrentMouseState;
        }

        private bool LeftMouseButtonPressed()
        {
            return mCurrentMouseState.LeftButton == ButtonState.Pressed;
        }

        private bool RightMouseButtonPressed()
        {
            return mCurrentMouseState.RightButton == ButtonState.Pressed;
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

        // Update current and previous MouseWheelValue
        private void UpdateMouseWheelValue()
        {
            mPreviousMouseWheelValue = mCurrentMouseWheelValue;
            mCurrentMouseWheelValue = mCurrentMouseState.ScrollWheelValue;
        }

        private void ClearActionList()
        {
            mInputState.mActionList.Clear();
        }

        private void ClearMouseAction()
        {
            mInputState.mMouseActionType = MouseActionType.None;
        }

        private void ClearKeyEventTypes()
        {
            mKeysKeyEventTypes.Clear();
        }

        // Updates all the inputs and returns actions and mouse position in InputState.
        public InputState Update()
        {
            SavePreviousMouseState();
            SavePreviousKeyState();
            ClearActionList();
            ClearKeyEventTypes();
            ClearMouseAction();
            UpdateMouseWheelValue();
            UpdateMouseState();
            UpdateKeyState();
            return mInputState;
        }
    }
}