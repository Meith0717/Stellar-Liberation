// Inventory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
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

        [JsonProperty] public readonly List<Item> ItemStacks = new();
        [JsonProperty] private readonly Dictionary<ItemID, List<Item>> keyValuePairs = new();

        public Inventory(int capacity = 30, int capacityPerStack = 16)
        {
            Capacity = capacity;
            CapacityPerStack = capacityPerStack;
        }

        [JsonIgnore] public int Count => ItemStacks.Count;

        public void Add(Item item)
        {
            if (item == null || item.Amount <= 0)
                return;

            if (!keyValuePairs.TryGetValue(item.ItemID, out var hashItems))
                keyValuePairs[item.ItemID] = hashItems = new();

            var stackCapacity = item.IsStakable ? CapacityPerStack : 1;
            foreach (var inItemStack in hashItems.Where(itemStack => itemStack.Amount < stackCapacity))
            {
                var addetAmount = stackCapacity - inItemStack.Amount;
                if (addetAmount > item.Amount) addetAmount = item.Amount;
                inItemStack.Amount += addetAmount;
                item.Amount -= addetAmount;
            }

            while (item.Amount > 0)
            {
                if (Count >= Capacity) break;

                var newStack = item.Split(stackCapacity);
                hashItems.Add(newStack);
                ItemStacks.Add(newStack);
                continue;
            }
        }

        public bool Remove(ItemID itemID, int amount)
        {
            if (amount <= 0 || GetItemCount(itemID, out var itemsStacks) < amount)
                return false;

            var itemsToRemove = itemsStacks
                .Reverse<Item>()
                .TakeWhile(itemStack =>
                {
                    if (itemStack.Amount <= amount)
                    {
                        amount -= itemStack.Amount;
                        return true;
                    }
                    itemStack.Amount -= amount;
                    amount = 0;
                    return false;
                })
                .ToList();

            itemsToRemove.ForEach(item =>
            {
                ItemStacks.Remove(item);
                keyValuePairs[itemID].Remove(item);
            });

            return true;
        }


        public void CheckForEmptyStacks(ItemID itemID)
        {
            keyValuePairs.TryGetValue(itemID, out var itemStacks);
            var emptyStacks = itemStacks.Where(itemStack => itemStack.Amount == 0).ToList();
            foreach (var item in emptyStacks)
            {
                ItemStacks.Remove(item);
                itemStacks.Remove(item);
            }
        }

        public void CheckForEmptyStacks()
        {
            foreach (var itemID in keyValuePairs.Keys) CheckForEmptyStacks(itemID);
        }

        private int GetItemCount(ItemID itemID, out List<Item> itemsStacks) =>
            keyValuePairs.TryGetValue(itemID, out itemsStacks) ? itemsStacks.Sum(itemStack => itemStack.Amount) : 0;

        public Dictionary<ItemID, int> GetItemsCount()
        {
            var dic = new Dictionary<ItemID, int>();
            foreach (var itemID in keyValuePairs.Keys) dic.Add(itemID, GetItemCount(itemID, out var _));
            return dic;
        }

        public bool HasSpace(Item item) => FreeSpace(item) >= item.Amount;

        private int FreeSpace(Item item)
        {
            var itemID = item.ItemID;
            keyValuePairs.TryGetValue(itemID, out var itemStacks);
            var stackCapacity = item.IsStakable ? CapacityPerStack : 1;
            var stackSpace = itemStacks is null ? 0 : itemStacks.Sum(itemStack => stackCapacity - itemStack.Amount);
            return stackSpace + (Capacity - ItemStacks.Count) * stackCapacity;
        }
    }
}
