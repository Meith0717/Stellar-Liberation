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

        public ItemStack Split(int amount)
        {
            if (Amount < amount) amount = Amount;
            mAmount -= amount;
            return new(ItemID, TextureID, amount);
        }

        public void Add(int amount = 1) => mAmount += amount;

        public void Remove(int amount = 1) => mAmount -= amount;

        public int Amount => mAmount;
    }
}
