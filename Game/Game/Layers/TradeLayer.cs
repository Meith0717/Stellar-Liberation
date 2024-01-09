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
using System.Linq;

namespace StellarLiberation.Game.Layers
{
    public class TradeLayer : Layer
    {
        private readonly TradingSystem mTradingSystem;
        private readonly Wallet mWallet;

        private readonly UiFrame mFrame;
        private readonly Inventory mPlayerInventory;
        private readonly Inventory mTraderInventory;
        private readonly Inventory mSellInventory;
        private readonly Inventory mBuyInventory;
        private readonly UiText mUiBalance;
        private readonly UiText mUIWallet;

        public TradeLayer(Inventory playerInventory, Inventory traderInventory, Wallet wallet)
            : base(false)
        {
            mWallet = wallet;
            mTradingSystem = new();
            mPlayerInventory = playerInventory;
            mTraderInventory = traderInventory;
            mSellInventory = new(8, 8);
            mBuyInventory = new(8, 8);

            mFrame = new() { Height = 900, Width = 1600, Anchor = Anchor.Center, Alpha = 0 };

            // Trading Layer
            var tradingLayer = new UiFrame() { Height = 800, Width = 450, Anchor = Anchor.Center };
            mFrame.AddChild(tradingLayer);
            var centerGrid = new UiGrid(1, 5) { Anchor = Anchor.S, Width = 450, Height = 775, VSpace = 20, HSpace = 20 };
            tradingLayer.AddChild(centerGrid);
            
            centerGrid.Set(0, 0, new UiText(FontRegistries.descriptionFont, "Items you give") { Anchor = Anchor.Center });
            centerGrid.Set(0, 1, new UiInventory(mSellInventory, RedoSell, 4, 2) { Anchor = Anchor.S, Width = 400, Height = 200 });
            centerGrid.Set(0, 2, new UiText(FontRegistries.descriptionFont, "Items you recive") { Anchor = Anchor.Center });
            centerGrid.Set(0, 3, new UiInventory(mBuyInventory, RedoBuy, 4, 2) { Anchor = Anchor.S, Width = 400, Height = 200 });
            
            var bottomGrid =  new UiGrid(1, 2) { Anchor = Anchor.Center, FillScale = FillScale.Both };
            centerGrid.Set(0, 4, bottomGrid);
            
            bottomGrid.Set(0, 1, new UiButton(MenueSpriteRegistries.button, "Trade") { Anchor = Anchor.Center, TextAllign = TextAllign.Center, OnClickAction = () => Trade(wallet)});
            bottomGrid.Set(0, 0, mUiBalance = new UiText(FontRegistries.textFont, "Balance: n.a") { Anchor = Anchor.Center });

            // Player Inventory
            var playerSide = new UiFrame() { Height = 800, Width = 550, Anchor = Anchor.W};
            mFrame.AddChild(playerSide);
            playerSide.AddChild(new UiText(FontRegistries.descriptionFont, "Your Items") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            playerSide.AddChild(new UiInventory(mPlayerInventory, Sell) { Width = 500, Height = 600, Anchor = Anchor.Center });
            var walletGrid = new UiGrid(2, 1) { Width = 200, Height = 50, Anchor = Anchor.SE };
            playerSide.AddChild(walletGrid);
            walletGrid.Set(0, 0, new UiText(FontRegistries.textFont, "Credits:") { Anchor = Anchor.Center });
            walletGrid.Set(1, 0, mUIWallet = new UiText(FontRegistries.textFont, wallet.Balance.ToString()) { Anchor = Anchor.Center });

            // Trader Inventory
            var traderSide = new UiFrame() { Height = 800, Width = 550, Anchor = Anchor.E};
            mFrame.AddChild(traderSide);
            traderSide.AddChild(new UiText(FontRegistries.descriptionFont, "Their Items") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            traderSide.AddChild(new UiInventory(mTraderInventory, Buy) { Width = 500, Height = 600, Anchor = Anchor.Center });
        }

        private void Sell(ItemStack itemStack)
        {
            if (!mSellInventory.HasSpace(itemStack.ItemID)) return;
            if (!mTraderInventory.HasSpace(itemStack.ItemID)) return;
            var newItemStack = itemStack.Split(1);
            mTradingSystem.AddSellingItem(newItemStack);
            mSellInventory.Add(newItemStack);
            mPlayerInventory.CheckForEmptyStacks(itemStack.ItemID);
        }

        private void RedoSell(ItemStack itemStack)
        {
            var newItemStack = itemStack.Split(1);
            mTradingSystem.RemoveSellingItem(newItemStack);
            mPlayerInventory.Add(newItemStack);
            mSellInventory.CheckForEmptyStacks(itemStack.ItemID);
        }

        private void Buy(ItemStack itemStack)
        {
            if (!mBuyInventory.HasSpace(itemStack.ItemID)) return;
            if (!mPlayerInventory.HasSpace(itemStack.ItemID)) return;
            var newItemStack = itemStack.Split(1);
            mTradingSystem.AddBuyingItem(newItemStack);
            mBuyInventory.Add(newItemStack);
            mTraderInventory.CheckForEmptyStacks(itemStack.ItemID);
        }

        private void RedoBuy(ItemStack itemStack)
        {
            var newItemStack = itemStack.Split(1);
            mTradingSystem.RemoveBuyingItem(newItemStack);
            mTraderInventory.Add(newItemStack);
            mBuyInventory.CheckForEmptyStacks(itemStack.ItemID);
        }

        private void Trade(Wallet wallet)
        {
            if (!mTradingSystem.TryToTrade(wallet)) return;
            foreach (var itemStack in mSellInventory.ItemStacks.ToList())
            {
                mTraderInventory.Add(itemStack);
                mSellInventory.CheckForEmptyStacks(itemStack.ItemID);
            }
            foreach (var itemStack in mBuyInventory.ItemStacks.ToList())
            {
                mPlayerInventory.Add(itemStack);
                mBuyInventory.CheckForEmptyStacks(itemStack.ItemID);
            }
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
            var balance = mTradingSystem.GetBalance();
            mUiBalance.Text = (balance > 0)? $"Balance: +{balance}": $"Balance: {balance}";
            mUIWallet.Text = mWallet.Balance.ToString();

            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);
            inputState.DoAction(ActionType.Trading, LayerManager.PopLayer);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }
    }
}
