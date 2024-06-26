// UiCheckBox.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiCheckBox : UiButton
    {
        public bool Value => mState;
        private bool mState;

        public UiCheckBox(bool initialState)
            : base(initialState ? "toggleOn" : "toggleOff", "", TextAllign.Center)
        {
            mState = initialState;
            OnClickAction = () => mState = !mState;
        }


        public override void Draw()
        {
            var spriteId = mState ? "toggleOn" : "toggleOff";
            TextureManager.Instance.Draw(spriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Color);
            Canvas.Draw();
        }
    }
}
