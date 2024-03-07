// Item.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;

namespace StellarLiberation.Game.GameObjects.Recources.Items
{
    [Serializable]
    public class Item
    {
        [JsonProperty] public readonly ItemID ItemID;
        [JsonProperty] public readonly string UiTextureId;
        [JsonProperty] public readonly bool IsStakable;
        [JsonProperty] public int Amount = 1;

        public Item(ItemID itemID, string textureId, bool isStackable = false)
        {
            ItemID = itemID;
            UiTextureId = textureId;
            IsStakable = isStackable;
        }

        private Item(ItemID itemID, string textureId, bool isStackable, int amount)
        {
            ItemID = itemID;
            UiTextureId = textureId;
            IsStakable = isStackable;
            Amount = amount;
        }


        public Item Split(int amount)
        {
            if (Amount < amount) amount = Amount;
            Amount -= amount;
            return new(ItemID, UiTextureId, IsStakable, amount);
        }
    }
}
