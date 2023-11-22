// UiButtonHoverTracer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.InputManagement
{
    public class UiButtonHoverTracer
    {
        private List<UiButton> mButtons = new();
        private int mIndex = -1;

        public void AddButton(UiButton uiButton) => mButtons.Add(uiButton);

        public void Trace(InputState inputState)
        {
            if (inputState.GamePadActions.Contains(GamePadActionType.RightThumbStickDown))
                mIndex = (mIndex + 1) % mButtons.Count;
            if (inputState.GamePadActions.Contains(GamePadActionType.RightThumbStickUp))
                mIndex = (mIndex - 1) % mButtons.Count;
            mIndex = mIndex < -1 ? mButtons.Count - 1 : mIndex;
            if (mIndex < 0) return;
            mButtons[mIndex].IsHover = true;
        }
    }
}
