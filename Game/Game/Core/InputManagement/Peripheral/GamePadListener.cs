// GamePadManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.InputManagement.Peripheral
{
    internal class GamePadListener
    {
        private readonly Dictionary<Buttons, ActionType> mActionOnPadPressed, mActionOnPadHold;
        private GamePadState mPreviousState, mCurrentState;

        public GamePadListener()
        {
            mActionOnPadPressed = new()
            {
                { Buttons.Start, ActionType.ESC },
                { Buttons.RightThumbstickDown, ActionType.None },
                { Buttons.RightThumbstickUp, ActionType.None },
            };

            mActionOnPadHold = new()
            {
                { Buttons.RightThumbstickUp, ActionType.CameraZoomOut },
                { Buttons.RightThumbstickDown, ActionType.CameraZoomIn },
                { Buttons.RightTrigger, ActionType.Accelerate },
                { Buttons.LeftTrigger, ActionType.Break },
                { Buttons.A, ActionType.FireInitialWeapon },
            };
        }

        private ThumbSticksState GetThumbSticksState()
        {
            var padValues = new ThumbSticksState();
            padValues.RightThumbSticks = new(mCurrentState.ThumbSticks.Right.X, -mCurrentState.ThumbSticks.Right.Y);
            padValues.LeftThumbSticks = new(mCurrentState.ThumbSticks.Left.X, -mCurrentState.ThumbSticks.Left.Y);
            padValues.RightTrigger = mCurrentState.Triggers.Right;
            padValues.LeftTrigger = mCurrentState.Triggers.Left;
            return padValues;
        }

        private List<Buttons> HoldButtons => Enum.GetValues(typeof(Buttons))
               .Cast<Buttons>()
               .Where(b => mCurrentState.IsButtonDown(b))
               .ToList();
        private List<Buttons> PressedButtons => Enum.GetValues(typeof(Buttons))
               .Cast<Buttons>()
               .Where(b => mCurrentState.IsButtonDown(b) && !mPreviousState.IsButtonDown(b))
               .ToList();

        public void Listen(ref List<ActionType> actions, out bool isConected, out ThumbSticksState thumbSticksState)
        {
            mPreviousState = mCurrentState;
            mCurrentState = GamePad.GetState(PlayerIndex.One);
            thumbSticksState = GetThumbSticksState();
            isConected = mCurrentState.IsConnected;
            foreach (Buttons button in PressedButtons)
            {
                if (mActionOnPadPressed.TryGetValue(button, out var actionPressed)) actions.Add(actionPressed);
            }
            foreach (Buttons button in HoldButtons)
            {
                if (mActionOnPadHold.TryGetValue(button, out var actionPressed)) actions.Add(actionPressed);
            }
        }
    }
}
