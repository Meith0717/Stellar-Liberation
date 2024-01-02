// SellLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;

namespace StellarLiberation.Game.Layers
{
    public class SellLayer : Layer
    {

        private readonly UiFrame mUiFrame;
        private readonly UiItemSlot mItemSlot;
        private readonly Inventory mInventory;

        public SellLayer(Inventory inventory) 
            : base(false)
        {
            mInventory = inventory;

            mUiFrame = new() { Anchor = Anchor.Center, Width = 900, Height = 700 };
            mUiFrame.AddChild(new UiInventory(inventory, null) { Width = 500, Height = 600, Anchor = Anchor.W, HSpace = 50 });
            var rightgrid = new UiGrid(3, 6) { Width = 300, Height = 600, Anchor = Anchor.E, HSpace = 50 };
            mUiFrame.AddChild(rightgrid);

            rightgrid.Set(1, 1, mItemSlot = new UiItemSlot(null));
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mUiFrame.Draw();
            spriteBatch.End();
        }
        public override void Destroy() { }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.PopLayer());
            mUiFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
