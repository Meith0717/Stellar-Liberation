// InputManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement.Peripheral;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.InputManagement
{
    public class InputManager
    {
        private KeyboardListener mKeyboardListener = new();
        private MouseListener mMouseListener = new();
        private GamePadListener mGamePadListener = new();

        public InputState Update(GameTime gameTime)
        {
            var actions = new List<ActionType>();

            mGamePadListener.Listen(ref actions, out var gamePadIsConnected, out var thumbSticksState);
            mMouseListener.Listen(gameTime, ref actions, out var mousePosition);
            mKeyboardListener.Listener(ref actions);
            return new(actions, mousePosition, gamePadIsConnected, thumbSticksState);
        }
    }
}