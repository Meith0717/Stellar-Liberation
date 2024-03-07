// TradingSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using StellarLiberation.Game.GameObjects.Recources.Items;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    internal class TradingSystem
    {
        private int mBalance;

        private int GetStackValue(Item itemStack)
        {
            var itemValue = ItemValues.GetValue(itemStack.ItemID);
            var itemAmount = itemStack.Amount;

            return itemValue * itemAmount;
        }
        public int GetBalance() => mBalance;

        public void AddSellingItem(Item item) => mBalance += GetStackValue(item);

        public void AddBuyingItem(Item item) => mBalance -= GetStackValue(item);
        public void RemoveSellingItem(Item item) => mBalance -= GetStackValue(item);

        public void RemoveBuyingItem(Item item) => mBalance += GetStackValue(item);

        public bool TryToTrade(Wallet wallet)
        {
            if (wallet.Balance + mBalance < 0) return false;
            wallet.Add(mBalance);
            mBalance = 0;
            return true;
        }
    }
}
