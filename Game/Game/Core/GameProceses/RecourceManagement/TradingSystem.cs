// TradingSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    internal class TradingSystem
    {
        private int mBalance;

        private int GetStackValue(ItemStack itemStack)
        {
            var itemValue = ItemValues.GetValue(itemStack.ItemID);
            var itemAmount = itemStack.Amount;

            return itemValue * itemAmount;
        }

        public int GetBalance() => mBalance;

        public void AddSellingItem(ItemStack itemStack) => mBalance += GetStackValue(itemStack);

        public void AddBuyingItem(ItemStack itemStack) => mBalance -= GetStackValue(itemStack);
        public void RemoveSellingItem(ItemStack itemStack) => mBalance -= GetStackValue(itemStack);

        public void RemoveBuyingItem(ItemStack itemStack) => mBalance += GetStackValue(itemStack);

        public bool TryToTrade(Wallet wallet)
        {
            if (wallet.Balance + mBalance < 0) return false;
            wallet.Add(mBalance);
            mBalance = 0;
            return true;
        }
    }
}
