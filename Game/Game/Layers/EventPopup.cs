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
    internal class EventPopup : Layer
    {
        private UiFrame mUiFrame;

        public EventPopup(string message) : base(false)
        {
            mUiFrame = new(50) { RelHeight = .2f, RelWidth = .2f, Color = Color.DarkGray, Anchor = Anchor.Center };
            mUiFrame.AddChild(new UiText(FontRegistries.subTitleFont, message) { Anchor = Anchor.Center});
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
