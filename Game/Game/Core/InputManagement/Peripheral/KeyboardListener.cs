
// KeyboardManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.InputManagement.Peripheral
{
    public class KeyboardListener
    {
        private readonly Dictionary<int, ActionType> mActionOnMultiplePressed;
        private readonly Dictionary<Keys, ActionType> mActionOnPressed, mActionOnHold;
        private readonly Dictionary<Keys, KeyEventType> mKeysKeyEventTypes;
        private Keys[] mCurrentKeysPressed, mPreviousKeysPressed;

        public KeyboardListener()
        {
            mActionOnMultiplePressed = new()
            {
                { Hash(Keys.LeftAlt, Keys.F12), ActionType.ToggleDebug },
                { Hash(Keys.LeftAlt, Keys.Enter), ActionType.ToggleFullscreen},
            };

            mActionOnPressed = new()
            {
                { Keys.M, ActionType.ToggleHyperMap },
                { Keys.Escape, ActionType.ESC },
                { Keys.F1, ActionType.F1 },
                { Keys.F2, ActionType.F2 },
                { Keys.F3, ActionType.F3 },
                { Keys.F4, ActionType.F4 },
                { Keys.F5, ActionType.F5 },
                { Keys.F6, ActionType.F6 },
                { Keys.F7, ActionType.F7 },
                { Keys.F8, ActionType.F8 },
                { Keys.F9, ActionType.F9 },
                { Keys.F10, ActionType.F10 },
            };

            mActionOnHold = new()
            {
                { Keys.Space, ActionType.FireInitialWeapon },
                { Keys.W, ActionType.Accelerate },                
                { Keys.S, ActionType.Break },
                { Keys.Q, ActionType.CameraZoomIn },
                { Keys.E, ActionType.CameraZoomOut }
            };
            mKeysKeyEventTypes = new();
        }

        private int Hash(params Keys[] keys)
        {
            int tmp = 0;
            Array.Sort(keys);
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                Keys key = keys[i];
                tmp += (int)key * (int)Math.Pow(1000, i);
            }
            return tmp;
        }

        private void UpdateKeysKeyEventTypes()
        {
            var keyboardState = Keyboard.GetState();
            mCurrentKeysPressed = keyboardState.GetPressedKeys();

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

        public void Listener(ref List<ActionType> actions)
        {
            mPreviousKeysPressed = mCurrentKeysPressed;
            mKeysKeyEventTypes.Clear();

            mCurrentKeysPressed = Keyboard.GetState().GetPressedKeys();
            UpdateKeysKeyEventTypes();

            if (mActionOnMultiplePressed.TryGetValue(Hash(mCurrentKeysPressed), out var action))
            {
               foreach (var key in mCurrentKeysPressed)
               {
                    if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonDown) actions.Add(action);
               }
            }

            foreach (var key in mCurrentKeysPressed)
            {
                if (mActionOnPressed.TryGetValue(key, out var actionPressed))
                {
                    if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonDown) actions.Add(actionPressed);
                }
                if (!mActionOnHold.TryGetValue(key, out var actionHold)) continue;
                if (mKeysKeyEventTypes[key] == KeyEventType.OnButtonPressed) actions.Add(actionHold);
            }
        }

    }
}
