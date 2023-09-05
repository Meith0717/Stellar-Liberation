/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using CelestialOdyssey.Game.Core.InputManagement.Peripheral;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.InputManagement
{
    public class InputManager
    {
        private KeyboardManager mKeyboardManager = new();
        private MouseManager mMouseManager = new();
        private GamePadManager mGamePadManager = new();

        private readonly Dictionary<Buttons, ActionType> mKeyBindingsButtonPressed, mKeyBindingsButtonHold;

        // Attributes keyboard. new.
        private Buttons[] mCurrentButtonPressed, mPreviousButtonPressed;


        // InputState contains all the actions made by the player and mouse position.
        private readonly InputState mInputState;

        // Constructor.
        public InputManager()
        {
            // Dictionary for keyboard keys that have been pressed and corresponding actions.
            mKeyBindingsButtonPressed = new()
            {
                {Buttons.Start, ActionType.ESC },
            };

            mKeyBindingsButtonHold = new() { };

            mInputState = new InputState();
        }

        // Updates all the inputs and returns actions and mouse position in InputState.
        public InputState Update()
        {
            mInputState.mActionList.Clear();
            mInputState.mMousePosition = mMouseManager.GetPosition();
            mInputState.mActionList.AddRange(mMouseManager.GetAction(out mInputState.mMouseActionType));
            mInputState.mActionList.AddRange(mGamePadManager.GetActions(out mInputState.mGamePadValues));
            mInputState.mActionList.AddRange(mKeyboardManager.GetActions());
            mInputState.mPrevGamePadValues = mInputState.mGamePadValues;

            return mInputState;
        }
    }
}