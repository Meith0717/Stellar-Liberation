// ItemStack.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Recources.Items;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    public class ItemStack
    {
        public readonly ItemID ItemID;
        public readonly string TextureID;
        private int mAmount;

        public ItemStack(ItemID itemID, string textureID, int amount)
        {
            ItemID = itemID;
            TextureID = textureID;
            mAmount = amount;
        }

        public bool TrySplit(int amount, out ItemStack itemStack)
        {
            itemStack = null;
            if (Amount <= amount) return false;
            itemStack = new(ItemID, TextureID, amount);
            mAmount -= amount;
            return true;
        }

        public void Add(int amount = 1) => mAmount += amount;

        public void Remove(int amount = 1) => mAmount -= amount;

        public int Amount => mAmount;
    }
}
