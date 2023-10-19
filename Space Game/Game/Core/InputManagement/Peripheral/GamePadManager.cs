// GamePadManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.InputManagement.Peripheral
{
    internal class GamePadManager
    {
        private readonly Dictionary<Buttons, ActionType> mActionOnPadPressed, mActionOnPadHold;
        private GamePadState mPreviousState, mState;

        public GamePadManager()
        {
            mActionOnPadPressed = new()
            {
                { Buttons.Start, ActionType.ESC },
            };

            mActionOnPadHold = new()
            {
                { Buttons.RightThumbstickUp, ActionType.CameraZoomIn },
                { Buttons.RightThumbstickDown, ActionType.CameraZoomOut },
                { Buttons.RightTrigger, ActionType.FireInitialWeapon },
                { Buttons.LeftTrigger, ActionType.FireSecondaryWeapon },
            };
        }

        private List<Buttons> GetHoldButtons(GamePadState gamePadState)
        {
            return Enum.GetValues(typeof(Buttons))
                       .Cast<Buttons>()
                       .Where(b => gamePadState.IsButtonDown(b))
                       .ToList();
        }
        public List<Buttons> GetPressedButtons(GamePadState gamePadState)
        {
            return Enum.GetValues(typeof(Buttons))
                       .Cast<Buttons>()
                       .Where(b => gamePadState.IsButtonDown(b) && !mPreviousState.IsButtonDown(b))
                       .ToList();
        }

        private void GetGamePadValues(out GamePadValues padValues)
        {
            padValues = new();
            padValues.mRightThumbSticks = new(mState.ThumbSticks.Right.X, -mState.ThumbSticks.Right.Y);
            padValues.mLeftThumbSticks = new(mState.ThumbSticks.Left.X, -mState.ThumbSticks.Left.Y);
            padValues.mRightTrigger = mState.Triggers.Right;
            padValues.mLeftTrigger = mState.Triggers.Left;
        }

        public List<ActionType> GetActions(out GamePadValues padValues)
        {
            GetGamePadValues(out padValues);
            List<ActionType> actions = new();
            mPreviousState = mState;
            mState = GamePad.GetState(PlayerIndex.One);
            foreach (Buttons button in GetPressedButtons(mState))
            {
                if (mActionOnPadPressed.TryGetValue(button, out var actionPressed))
                {
                    actions.Add(actionPressed);
                }
            }
            foreach (Buttons button in GetHoldButtons(mState))
            {
                if (mActionOnPadHold.TryGetValue(button, out var actionPressed))
                {
                    actions.Add(actionPressed);
                }
            }
            return actions;
        }
    }
}
