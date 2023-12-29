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

        [JsonProperty] public readonly List<ItemStack> Items = new();
        [JsonProperty] private readonly Dictionary<ItemID, List<ItemStack>> keyValuePairs = new();

        public Inventory()
        {
            CapacityPerStack = 16;
            Capacity = 30;
        }

        [JsonIgnore] public int Count => Items.Count;

        public bool Add(Item item)
        {

            if (!keyValuePairs.TryGetValue(item.ItemID, out var itemStacks)) 
                keyValuePairs.Add(item.ItemID, itemStacks = new());

            foreach (var stack in itemStacks)
            {
                if (stack.Count >= CapacityPerStack) continue;   
                stack.Add(item);
                return true;
            }

            var newStack = new ItemStack(item.ItemID, item.TextureId);
            newStack.Add(item);
            itemStacks.Add(newStack);
            Items.Add(newStack);
            return true;
        }

        public bool Remove(ItemID itemID, int amount)
        {
            if (amount <= 0) return true;
            if (GetItemCount(itemID, out var itemStacks) < amount) return false;

            var itemsToRemove = new List<ItemStack>();

            foreach (var stack in itemStacks.Reverse<ItemStack>())
            {
                if (stack.Count <= amount)
                {
                    amount -= stack.Count;
                    itemsToRemove.Add(stack);
                } else
                {
                    stack.Remove(amount);
                    amount = 0;
                }
            }

            foreach (var stackToRemove in itemsToRemove)
            {
                Items.Remove(stackToRemove);
                keyValuePairs[itemID].Remove(stackToRemove);
            }

            return true;
        }


        private int GetItemCount(ItemID itemID, out List<ItemStack> itemStacks)
        {
            if (!keyValuePairs.TryGetValue(itemID, out itemStacks))
                return 0;

            return itemStacks.Sum(stack => stack.Count);
        }


        public bool HasSpace(ItemID itemID)
        {
            if (Count < Capacity) return true;
            if (keyValuePairs.TryGetValue(itemID, out var itemStacks)) 
            {
                foreach (var inventoryItem in itemStacks)
                {
                    if (inventoryItem.Count >= CapacityPerStack) continue;
                    return true;
                }
            }
            return false;
        }
    }
}
