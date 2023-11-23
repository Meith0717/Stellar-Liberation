// UiButtonHoverTracer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.InputManagement
{
    public class ButtonInputTracer
    {
        private List<UiButton> mButtons = new();
        private int mIndex = 0;

        public void AddButton(UiButton uiButton) => mButtons.Add(uiButton);

        public void Trace(InputState inputState)
        {

            inputState.DoAction(ActionType.MoveButtonDown, () => mIndex = (mIndex + 1) % mButtons.Count);
            inputState.DoAction(ActionType.MoveButtonUp, () => mIndex = (mIndex - 1) % mButtons.Count);
            mIndex = mIndex < 0 ? mButtons.Count - 1 : mIndex;

            for (int i = 0; i < mButtons.Count; i++)
            {
                var button = mButtons[i];
                if (button.IsDisabled) continue;

                button.IsHover = false;
                if ((inputState.GamePadIsConnected && i == mIndex) || button.Contains(inputState.mMousePosition)) button.IsHover = true;
            }
        }
    }
}
