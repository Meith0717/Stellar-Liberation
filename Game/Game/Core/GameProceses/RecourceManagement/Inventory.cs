// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{

    public class ItemStack
    {
        public int Amount;
        public readonly string ItemName;
        public readonly string Texture;
        public ItemStack(int amount, string itemName, string texture)
        {
            Amount = amount; ItemName = itemName; Texture = texture;
        }
    }

    [Serializable]
    public class Inventory
    {
        [JsonProperty] public int Count { get; private set; }
        [JsonProperty] public int Capacity { get; private set; }

        [JsonProperty] public readonly Dictionary<ItemID, ItemStack> Items = new();

        public Inventory(int capacity) => Capacity = capacity;

        public void Collect(Item item)
        {
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.collect);

            // Add Item in Inventory
            if (!Items.TryGetValue(item.ItemID, out var stack))
            {
                Items[item.ItemID] = new(1, item.ItemID.ToString(), item.TextureId);
            }
            else
            {
                stack.Amount++;
            }

            // Some Other stuff
            item.Dispose = true; ;
            Count++;
        }

        public bool RemoveItemAmount(ItemID itemID, int amount)
        {
            if (!Items.TryGetValue(itemID, out var stack)) return false;
            if (stack.Amount < amount) return false;
            stack.Amount -= amount;
            if (stack.Amount == 0) Items.Remove(itemID);
            return true;
        }

        public int GetIDAmount(ItemID itemID)
        {
            if (!Items.TryGetValue(itemID, out var stack)) return 0;
            return stack.Amount;
        }
    }
}
