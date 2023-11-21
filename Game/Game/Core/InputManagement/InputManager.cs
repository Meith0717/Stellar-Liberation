// InputManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


using StellarLiberation.Game.Core.InputManagement.Peripheral;

namespace StellarLiberation.Game.Core.InputManagement
{
    public class InputManager
    {
        private KeyboardManager mKeyboardManager = new();
        private MouseManager mMouseManager = new();
        private GamePadManager mGamePadManager = new();

        public InputState Update()
        {
            var actions = mGamePadManager.GetActions(out var gamePadIsConnected, out var gamePadValues);
            var mousePosition = mMouseManager.GetPosition();
            actions.AddRange(mMouseManager.GetAction(out var mouseActions));
            actions.AddRange(mKeyboardManager.GetActions());
            System.Diagnostics.Debug.WriteLine(gamePadValues.mLeftThumbSticks.ToString());
            return new InputState(actions, mouseActions, mousePosition, gamePadValues);
        }
    }
}