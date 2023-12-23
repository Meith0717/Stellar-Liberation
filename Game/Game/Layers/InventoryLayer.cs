﻿// InventoryLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    public class InventoryLayer : Layer
    {
        private readonly UiFrame mFrame;

        private class StackLine : UiFrame
        {
            public StackLine() : base(0)
            {
                Anchor = Anchor.CenterV;
                Height = 70;
                Width = 70;
                Color = Color.DarkGray;
            }
        }

        public InventoryLayer(Inventory inventory) : base(false)
        {
            mFrame = new(50) { Height = 500, Width = 800, Anchor = Anchor.Center, Color = new(17, 17, 17) };
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.Inventar, mLayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.ResolutionManager.UiScaling);
        }
    }
}
