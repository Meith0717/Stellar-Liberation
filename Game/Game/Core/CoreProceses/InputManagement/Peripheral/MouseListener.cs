﻿// MouseListener.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.InputManagement.Peripheral
{
    internal class MouseListener
    {
        private const double mClickHoldTeshholld = 75;
        private MouseState mCurrentState, mPreviousState;
        private double mLeftCounter, mRightCounter;

        private bool LeftMouseButtonPressed => mCurrentState.LeftButton == ButtonState.Pressed;
        private bool RightMouseButtonPressed => mCurrentState.RightButton == ButtonState.Pressed;

        private bool LeftMouseButtonReleased => mCurrentState.LeftButton == ButtonState.Released;
        private bool RightMouseButtonReleased => mCurrentState.RightButton == ButtonState.Released;

        private bool LeftMouseButtonJustReleased => mCurrentState.LeftButton == ButtonState.Released && mPreviousState.LeftButton == ButtonState.Pressed;
        private bool RightMouseButtonJustReleased => mCurrentState.RightButton == ButtonState.Released && mPreviousState.RightButton == ButtonState.Pressed;


        private readonly Dictionary<ActionType, ActionType> mKeyBindingsMouse = new()
            {
                { ActionType.MouseWheelBackward, ActionType.CameraZoomOut },
                { ActionType.MouseWheelForward, ActionType.CameraZoomIn },
                { ActionType.LeftClickReleased, ActionType.Select },
            };

        public void Listen(GameTime gameTime, ref List<ActionType> actions, out Vector2 mousePosition)
        {
            mPreviousState = mCurrentState;
            mCurrentState = Mouse.GetState();
            mousePosition = mCurrentState.Position.ToVector2();

            // Track the time the Keys are Pressed
            if (LeftMouseButtonPressed) 
                mLeftCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (RightMouseButtonPressed)
                mRightCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check if Mouse Key was Hold or Clicked
            if (mLeftCounter > mClickHoldTeshholld)
                actions.Add(ActionType.LeftClickHold);
            else
                if (LeftMouseButtonJustReleased)
                    actions.Add(ActionType.LeftClick);

            if (mRightCounter > mClickHoldTeshholld)
                actions.Add(ActionType.RightClickHold);
            else
                if (RightMouseButtonJustReleased)
                    actions.Add(ActionType.RightClick);

            // Check for Mouse Key Release
            if (LeftMouseButtonJustReleased)
                actions.Add(ActionType.LeftClickReleased);

            if (RightMouseButtonJustReleased)
                actions.Add(ActionType.RightClickReleased);

            // Recet counters
            if (LeftMouseButtonReleased)
                mLeftCounter = 0;

            if (RightMouseButtonReleased)
                mRightCounter = 0;

            // Set Mouse Action to MouseWheel
            if (mCurrentState.ScrollWheelValue > mPreviousState.ScrollWheelValue)
                actions.Add(ActionType.MouseWheelForward);

            if (mCurrentState.ScrollWheelValue < mPreviousState.ScrollWheelValue)
                actions.Add(ActionType.MouseWheelBackward);

            foreach (var key in mKeyBindingsMouse.Keys)
            {
                if (!actions.Contains(key)) continue;
                actions.Add(mKeyBindingsMouse[key]);
            }
        }
    }
}
