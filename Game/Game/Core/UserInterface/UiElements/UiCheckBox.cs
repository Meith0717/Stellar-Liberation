// UiCheckBox.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public class UiCheckBox : UiButton
    {
        public bool Value => mState;
        private bool mState;

        public UiCheckBox(bool initialState)
            : base(initialState ? MenueSpriteRegistries.toggleOn : MenueSpriteRegistries.toggleOff, "", TextAllign.Center)
        {
            mState = initialState;
            OnClickAction = () => mState = !mState;
        }


        public override void Draw()
        {
            var spriteId = mState ? MenueSpriteRegistries.toggleOn : MenueSpriteRegistries.toggleOff;
            TextureManager.Instance.Draw(spriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, Color);
            Canvas.Draw();
        }
    }
}
