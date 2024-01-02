// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    [Serializable]
    public class Inventory
    {
        [JsonProperty] public int CapacityPerStack { get; private set; }
        [JsonProperty] public int Capacity { get; private set; }

        [JsonProperty] public readonly List<Item> Items = new();
        [JsonProperty] private readonly Dictionary<ItemID, List<Item>> keyValuePairs = new();

        public Inventory(int capacity = 30)
        {
            CapacityPerStack = 16;
            Capacity = capacity;
        }

        [JsonIgnore] public int Count => Items.Count;

        public void Add(Item item)
        {
            item.Dispose = true;

            if (!keyValuePairs.TryGetValue(item.ItemID, out var hashItems)) 
                keyValuePairs.Add(item.ItemID, hashItems = new());

            foreach (var inItem in hashItems)
            {
                if (inItem.Amount >= CapacityPerStack) continue;   
                inItem.Amount++;
                return;
            }

            hashItems.Add(item);
            Items.Add(item);
        }

        public bool Remove(ItemID itemID, int amount)
        {
            if (amount <= 0) return true;
            if (GetItemCount(itemID, out var items) < amount) return false;

            var itemsToRemove = new List<Item>();

            foreach (var item in items.Reverse<Item>())
            {
                if (item.Amount <= amount)
                {
                    amount -= item.Amount;
                    itemsToRemove.Add(item);
                } else
                {
                    item.Amount -= amount;
                    amount = 0;
                }
            }

            foreach (var item in itemsToRemove)
            {
                Items.Remove(item);
                keyValuePairs[itemID].Remove(item);
            }

            return true;
        }


        private int GetItemCount(ItemID itemID, out List<Item> items)
        {
            if (!keyValuePairs.TryGetValue(itemID, out items))
                return 0;

            return items.Sum(item => item.Amount);
        }


        public bool HasSpace(ItemID itemID)
        {
            if (Count < Capacity) return true;
            if (keyValuePairs.TryGetValue(itemID, out var items)) 
            {
                foreach (var item in items)
                {
                    if (item.Amount >= CapacityPerStack) continue;
                    return true;
                }
            }
            return false;
        }
    }
}
