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
using System;

namespace StellarLiberation.Game.Layers
{
    internal class EventPopup : Layer
    {
        private readonly UiFrame mUiFrame;

        public EventPopup(string message, Action onYes, Action onCancel) : base(false)
        {
            mUiFrame = new() { Height = 200, Width = 500, Anchor = Anchor.Center };
            mUiFrame.AddChild(new UiText(FontRegistries.textFont, message) { Anchor = Anchor.Center});
            mUiFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Yes") { TextAllign = TextAllign.W, OnClickAction = () => { onYes?.Invoke(); LayerManager.PopLayer(); }, Anchor = Anchor.SW, HSpace = 10, VSpace = 10 });
            mUiFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Cancel") { TextAllign = TextAllign.E, OnClickAction = () => { onCancel?.Invoke(); LayerManager.PopLayer(); }, Anchor = Anchor.SE, HSpace = 10, VSpace = 10 });
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
