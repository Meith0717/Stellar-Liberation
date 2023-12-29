﻿// InventoryLayer.cs
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;

namespace StellarLiberation.Game.Layers
{
    public class InventoryLayer : Layer
    {
        private readonly Inventory mInventory;
        private readonly UiFrame mFrame;
        private readonly UiGrid mGrid;

        public InventoryLayer(Inventory inventory, Wallet wallet) : base(false)
        {
            mFrame = new() { Height = 800, Width = 550, Anchor = Anchor.Center };
            mFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Inventory") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            mFrame.AddChild(mGrid = new(5, 6) { Width = 500, Height = 600, Anchor = Anchor.Center });
            var walletGrid = new UiGrid(2, 1) { Width = 200, Height = 50, Anchor = Anchor.SE };
            mFrame.AddChild(walletGrid);
            walletGrid.Set(0, 0, new UiText(FontRegistries.textFont, "Credits:") { Anchor = Anchor.Center });
            walletGrid.Set(1, 0, new UiText(FontRegistries.textFont, wallet.Balance.ToString()) { Anchor = Anchor.Center });

            mInventory = inventory;
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
            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);

            var i = 0;
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (i < mInventory.Items.Count)
                    {
                        var itemStack = mInventory.Items[i];
                        mGrid.Set(x, y, new UiStackSlot(null, itemStack));
                        i++;
                    }
                    else
                    { mGrid.Set(x, y, new UiStackSlot()); }
                }
            }

            inputState.DoAction(ActionType.Inventar, LayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
