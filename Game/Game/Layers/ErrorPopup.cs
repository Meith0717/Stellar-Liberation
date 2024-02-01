// EventPopup.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    internal class ErrorPopup : Layer
    {
        private readonly UiFrame mUiFrame;

        public ErrorPopup(string title, string message) : base(false)
        {
            mUiFrame = new() { Height = 800, Width = 1500, Anchor = Anchor.Center };
            mUiFrame.AddChild(new UiText(FontRegistries.subTitleFont, title) { Anchor = Anchor.NW, HSpace = 20, VSpace = 20 });
            mUiFrame.AddChild(new UiText(FontRegistries.debugFont, message) { Anchor = Anchor.Center });
            //mUiFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Ok") { OnClickAction = LayerManager.PopLayer, Anchor = Anchor.SW, HSpace = 20, VSpace = 20});
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mUiFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mUiFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
