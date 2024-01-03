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

        public Inventory(int capacity = 30, int capacityPerStack = 16)
        {
            Capacity = capacity;
            CapacityPerStack = capacityPerStack;
        }

        [JsonIgnore] public int Count => Items.Count;

        public void Add(Item item)
        {
            item.Dispose = true;

            if (!keyValuePairs.TryGetValue(item.ItemID, out var hashItems))
                keyValuePairs[item.ItemID] = hashItems = new();

            if (!hashItems.Any(inItem => inItem.Amount < CapacityPerStack))
            {
                var stack = new ItemStack(item.ItemID, item.TextureId, 1);
                hashItems.Add(stack);
                Items.Add(stack);
            }
            else
            {
                hashItems.First(inItem => inItem.Amount < CapacityPerStack).Add();
            }
        }

        public void Add(ItemStack itemStack)
        {
            if (itemStack == null || itemStack.Amount <= 0)
                return;

            if (!keyValuePairs.TryGetValue(itemStack.ItemID, out var hashItems))
                keyValuePairs[itemStack.ItemID] = hashItems = new();

            foreach (var inItemStack in hashItems.Where(itemStack => itemStack.Amount < CapacityPerStack))
            {
                var addetAmount = CapacityPerStack - inItemStack.Amount;
                if (addetAmount > itemStack.Amount) addetAmount = itemStack.Amount;
                inItemStack.Add(addetAmount);
                itemStack.Remove(addetAmount);
            }

            while (itemStack.Amount > 0)
            {
                if (Count >= Capacity) break;

                if (itemStack.TrySplit(CapacityPerStack, out var newStack))
                {
                    hashItems.Add(newStack);                                
                    Items.Add(newStack);
                    continue;
                }

                hashItems.Add(itemStack);                                   
                Items.Add(itemStack);
                break;
            }
        }

        public bool Remove(ItemID itemID, int amount)
        {
            if (amount <= 0 || GetItemCount(itemID, out var itemsStacks) < amount)
                return false;

            var itemsToRemove = itemsStacks
                .Reverse<ItemStack>()
                .TakeWhile(itemStack =>
                {
                    if (itemStack.Amount <= amount)
                    {
                        amount -= itemStack.Amount;
                        return true;
                    }
                    itemStack.Remove(amount);
                    amount = 0;
                    return false;
                })
                .ToList();

            itemsToRemove.ForEach(item =>
            {
                Items.Remove(item);
                keyValuePairs[itemID].Remove(item);
            });

            return true;
        }

        public void Remove(ItemStack itemStack)
        {
            Items.Remove(itemStack);
            keyValuePairs.TryGetValue(itemStack.ItemID, out var itemStacks);
            itemStacks.Remove(itemStack);
        }

        private int GetItemCount(ItemID itemID, out List<ItemStack> itemsStacks) =>
            keyValuePairs.TryGetValue(itemID, out itemsStacks) ? itemsStacks.Sum(itemStack => itemStack.Amount) : 0;

        public Dictionary<ItemID, int> GetItemsCount()
        {
            var dic = new Dictionary<ItemID, int>();
            foreach (var itemID in keyValuePairs.Keys) dic.Add(itemID, GetItemCount(itemID, out var _));
            return dic;
        }

        public bool HasSpace(ItemID itemID, int amount = 1) => FreeSpace(itemID) >= amount;
            
        private int FreeSpace(ItemID itemID)
        {
            keyValuePairs.TryGetValue(itemID, out var itemStacks);
            var stackSpace = itemStacks is null ? 0 : itemStacks.Sum(itemStack => CapacityPerStack - itemStack.Amount);
            return stackSpace + (Capacity - Items.Count) * CapacityPerStack;
        }
    }
}
