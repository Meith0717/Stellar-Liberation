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
                { Keys.A, ActionType.None },
                { Keys.B, ActionType.None },
                { Keys.C, ActionType.None },
                { Keys.D, ActionType.None },
                { Keys.E, ActionType.None },
                { Keys.F, ActionType.None },
                { Keys.G, ActionType.None },
                { Keys.H, ActionType.None },
                { Keys.I, ActionType.None },
                { Keys.J, ActionType.None },
                { Keys.K, ActionType.None },
                { Keys.L, ActionType.Load },
                { Keys.M, ActionType.None },
                { Keys.N, ActionType.None },
                { Keys.O, ActionType.None },
                { Keys.P, ActionType.None },
                { Keys.Q, ActionType.None },
                { Keys.R, ActionType.None },
                { Keys.S, ActionType.Save },
                { Keys.T, ActionType.None },
                { Keys.U, ActionType.None },
                { Keys.V, ActionType.None },
                { Keys.W, ActionType.None },
                { Keys.X, ActionType.None },
                { Keys.Y, ActionType.None },
                { Keys.Z, ActionType.None },

                { Keys.Escape, ActionType.Back },
                { Keys.F1, ActionType.F1 },
                { Keys.F2, ActionType.F2 },
                { Keys.F3, ActionType.F3 },
                { Keys.F7, ActionType.F7 },
                { Keys.F8, ActionType.F8 },
                { Keys.F9, ActionType.F9 },
                { Keys.F10, ActionType.F10 },
                { Keys.F11, ActionType.ToggleFullscreen},
                { Keys.F12, ActionType.ToggleDebugModes},
            };

            mActionOnKeyboardHold = new()
            {
                { Keys.F4, ActionType.F4 },
                { Keys.F5, ActionType.F5 },
                { Keys.F6, ActionType.F6 },
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
