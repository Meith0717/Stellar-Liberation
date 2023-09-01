using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.InputManagement.Peripheral
{
    public class KeyboardManager
    {
        private readonly Dictionary<Keys, ActionType> mActionOnKeyboardPressed, mActionOnKeyboardHold;
        private readonly Dictionary<Keys, KeyEventType> mKeysKeyEventTypes;
        private Keys[] mCurrentKeysPressed, mPreviousKeysPressed;

        public KeyboardManager()
        {
            mActionOnKeyboardPressed = new()
            {
                { Keys.Escape, ActionType.ESC },
                { Keys.F11, ActionType.ToggleFullscreen},
                { Keys.F12, ActionType.ToggleDebugModes},
                { Keys.M, ActionType.ToggleMap},
            };

            mActionOnKeyboardHold = new()
            {
                { Keys.LeftShift, ActionType.Acceleration },
                { Keys.LeftControl, ActionType.Deacceleration },
                { Keys.Space, ActionType.FireInitialWeapon },
            };
            mKeysKeyEventTypes = new();
        }

        private void UpdateKeysKeyEventTypes()
        {
            mCurrentKeysPressed = Keyboard.GetState().GetPressedKeys();

            // Get KeyEventTypes (down or pressed) for keys.
            foreach (var key in mCurrentKeysPressed)
            {
                if (mPreviousKeysPressed == null)
                {
                    continue;
                }
                if (mPreviousKeysPressed.Contains(key))
                {
                    mKeysKeyEventTypes.Add(key, KeyEventType.OnButtonPressed);
                    continue;
                }
                mKeysKeyEventTypes.Add(key, KeyEventType.OnButtonDown);
            }
        }

        public List<ActionType> GetActions()
        {
            List<ActionType> actions = new();

            mPreviousKeysPressed = mCurrentKeysPressed;
            mKeysKeyEventTypes.Clear();

            mCurrentKeysPressed = Keyboard.GetState().GetPressedKeys();
            UpdateKeysKeyEventTypes();

            foreach (var key in mCurrentKeysPressed)
            {
                if (mActionOnKeyboardPressed.TryGetValue(key, out var actionPressed))
                {
                    if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonDown)
                    {
                        actions.Add(actionPressed);
                    }
                }
                if (!mActionOnKeyboardHold.TryGetValue(key, out var actionHold)) continue;
                if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonPressed)
                {
                    actions.Add(actionHold);
                }
            }
            return actions;
        }

    }
}
