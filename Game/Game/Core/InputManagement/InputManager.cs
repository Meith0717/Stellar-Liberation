// InputManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
    Copyright 2023 Thierry Meiers

    Code from the "Rache der RETI" project.
    https://meith0717.itch.io/rache-der-reti
*/

using StellarLiberation.Game.Core.InputManagement.Peripheral;

namespace StellarLiberation.Game.Core.InputManagement
{
    public class InputManager
    {
        private KeyboardManager mKeyboardManager = new();
        private MouseManager mMouseManager = new();
        private GamePadManager mGamePadManager = new();

        private readonly InputState mInputState = new();

        public InputState Update()
        {
            mInputState.mActions.Clear();
            mInputState.mMousePosition = mMouseManager.GetPosition();
            mInputState.mActions.AddRange(mMouseManager.GetAction(out mInputState.mMouseActions));
            mInputState.mActions.AddRange(mGamePadManager.GetActions(out mInputState.mGamePadValues));
            mInputState.mActions.AddRange(mKeyboardManager.GetActions());
            mInputState.mPrevGamePadValues = mInputState.mGamePadValues;
            return mInputState;
        }
    }
}