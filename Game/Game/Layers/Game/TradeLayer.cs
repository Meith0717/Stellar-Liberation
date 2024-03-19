// TradeLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System.Linq;

namespace StellarLiberation.Game.Layers.GameLayers
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

        public TradeLayer(Inventory playerInventory, Inventory traderInventory, Wallet wallet, Game1 game1)
            : base(game1, false)
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

            var bottomGrid = new UiGrid(1, 2) { Anchor = Anchor.Center, FillScale = FillScale.Both };
            centerGrid.Set(0, 4, bottomGrid);

            bottomGrid.Set(0, 1, new UiButton(MenueSpriteRegistries.button, "Trade") { Anchor = Anchor.Center, OnClickAction = () => Trade(wallet) });
            bottomGrid.Set(0, 0, mUiBalance = new UiText(FontRegistries.textFont, "Balance: n.a") { Anchor = Anchor.Center });

            // Player Inventory
            var playerSide = new UiFrame() { Height = 800, Width = 550, Anchor = Anchor.W };
            mFrame.AddChild(playerSide);
            playerSide.AddChild(new UiText(FontRegistries.descriptionFont, "Your Items") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            playerSide.AddChild(new UiInventory(mPlayerInventory, Sell) { Width = 500, Height = 600, Anchor = Anchor.Center });
            var walletGrid = new UiGrid(2, 1) { Width = 200, Height = 50, Anchor = Anchor.SE };
            playerSide.AddChild(walletGrid);
            walletGrid.Set(0, 0, new UiText(FontRegistries.textFont, "Credits:") { Anchor = Anchor.Center });
            walletGrid.Set(1, 0, mUIWallet = new UiText(FontRegistries.textFont, wallet.Balance.ToString()) { Anchor = Anchor.Center });

            // Trader Inventory
            var traderSide = new UiFrame() { Height = 800, Width = 550, Anchor = Anchor.E };
            mFrame.AddChild(traderSide);
            traderSide.AddChild(new UiText(FontRegistries.descriptionFont, "Their Items") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            traderSide.AddChild(new UiInventory(mTraderInventory, Buy) { Width = 500, Height = 600, Anchor = Anchor.Center });
        }

        private void Sell(Item item)
        {
            if (!mSellInventory.HasSpace(item)) return;
            if (!mTraderInventory.HasSpace(item)) return;
            var newItemStack = item.Split(1);
            mTradingSystem.AddSellingItem(item);
            mSellInventory.Add(newItemStack);
            mPlayerInventory.CheckForEmptyStacks(item.ItemID);
        }

        private void RedoSell(Item item)
        {
            var newItemStack = item.Split(1);
            mTradingSystem.RemoveSellingItem(item);
            mPlayerInventory.Add(newItemStack);
            mSellInventory.CheckForEmptyStacks(item.ItemID);
        }

        private void Buy(Item item)
        {
            if (!mBuyInventory.HasSpace(item)) return;
            if (!mPlayerInventory.HasSpace(item)) return;
            var newItemStack = item.Split(1);
            mTradingSystem.AddBuyingItem(item);
            mBuyInventory.Add(newItemStack);
            mTraderInventory.CheckForEmptyStacks(item.ItemID);
        }

        private void RedoBuy(Item item)
        {
            var newItemStack = item.Split(1);
            mTradingSystem.RemoveBuyingItem(item);
            mTraderInventory.Add(newItemStack);
            mBuyInventory.CheckForEmptyStacks(item.ItemID);
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

        public override void ApplyResolution() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            var balance = mTradingSystem.GetBalance();
            mUiBalance.Text = balance > 0 ? $"Balance: +{balance}" : $"Balance: {balance}";
            mUIWallet.Text = mWallet.Balance.ToString();

            inputState.DoAction(ActionType.ESC, LayerManager.PopLayer);
            inputState.DoAction(ActionType.Trading, LayerManager.PopLayer);
            mFrame.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
        }
    }
}
