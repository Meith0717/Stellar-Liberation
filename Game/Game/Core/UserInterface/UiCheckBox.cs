// UiCheckBox.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement;

namespace StellarLiberation.Game.Core.UserInterface
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
            TextureManager.Instance.Draw(spriteId, Position, Bounds.Width, Bounds.Height, Color);
            DrawCanvas();
        }
    }
}
