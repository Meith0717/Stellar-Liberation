// SellLayer.cs 
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
    public class TradeLayer : Layer
    {
        private readonly UiFrame mFrame;
        private readonly Inventory mPlayerInventory;
        private readonly Inventory mTraderInventory;
        private readonly Inventory mSellInventory;
        private readonly Inventory mBuyInventory;

        public TradeLayer(Inventory playerInventory, Inventory traderInventory, Wallet wallet)
            : base(false)
        {
            mPlayerInventory = playerInventory;
            mTraderInventory = traderInventory;
            mSellInventory = new(8, 8);
            mBuyInventory = new(8, 8);

            mFrame = new() { Height = 900, Width = 1600, Anchor = Anchor.Center};
            mFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Trading") { Anchor = Anchor.NW, VSpace = 20, HSpace = 20 });
            mFrame.AddChild(new UiInventory(mSellInventory, BreakSell, 4, 2) { Anchor = Anchor.CenterV, Width = 400, Height = 200, RelY = .25f});
            mFrame.AddChild(new UiInventory(mBuyInventory, null, 4, 2) { Anchor = Anchor.CenterV, Width = 400, Height = 200, RelY = .6f});

            // Player Inventory
            var playerSide = new UiFrame() { Height = 800, Width = 550, Anchor = Anchor.SW, VSpace = 20, HSpace = 20, Alpha = 0};
            mFrame.AddChild(playerSide);
            playerSide.AddChild(new UiText(FontRegistries.subTitleFont, "Inventory") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            playerSide.AddChild(new UiInventory(mPlayerInventory, SellItems) { Width = 500, Height = 600, Anchor = Anchor.Center });
            var walletGrid = new UiGrid(2, 1) { Width = 200, Height = 50, Anchor = Anchor.SE };
            playerSide.AddChild(walletGrid);
            walletGrid.Set(0, 0, new UiText(FontRegistries.textFont, "Credits:") { Anchor = Anchor.Center });
            walletGrid.Set(1, 0, new UiText(FontRegistries.textFont, wallet.Balance.ToString()) { Anchor = Anchor.Center });

            // Trader Inventory
            var traderSide = new UiFrame() { Height = 800, Width = 550, Anchor = Anchor.SE, VSpace = 20, HSpace = 20, Alpha = 0};
            mFrame.AddChild(traderSide);
            traderSide.AddChild(new UiText(FontRegistries.subTitleFont, "Trader") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            traderSide.AddChild(new UiInventory(mTraderInventory, null) { Width = 500, Height = 600, Anchor = Anchor.Center });
        }

        private void SellItems(ItemStack itemStack)
        {
            var newItemStack = itemStack.Split(1);
            mSellInventory.Add(newItemStack);
            mPlayerInventory.CheckForEmptyStacks(itemStack.ItemID);
        }

        private void BreakSell(ItemStack itemStack)
        {
            var newItemStack = itemStack.Split(1);
            mPlayerInventory.Add(newItemStack);
            mSellInventory.CheckForEmptyStacks(itemStack.ItemID);
        }

        public override void Destroy() 
        {
            foreach (var itemStack in mSellInventory.ItemStacks) mPlayerInventory.Add(itemStack);
        }

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
            inputState.DoAction(ActionType.Trading, LayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
