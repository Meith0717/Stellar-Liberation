// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    [Serializable]
    public class Inventory
    {
        [JsonProperty] public int StackCapacity { get; private set; }
        [JsonProperty] public int Capacity { get; private set; }

        [JsonProperty] public readonly List<InventoryItem> Items = new();

        public Inventory()
        {
            StackCapacity = 5;
            Capacity = 25;
        }

        [JsonIgnore] public int Count => Items.Count;

        public bool Add(Item item)
        {
            if (Count >= Capacity) return false;

            // Add Item to existing Stack
            var itemID = item.ItemID;
            foreach (var inventoryItem in Items.Where((item) => item.ItemId == itemID))
            {
                if (inventoryItem.Count >= StackCapacity) continue;
                inventoryItem.Add();
                item.Dispose = true;
                return true;
            }
            
            // Add new Stack
            Items.Add(new(itemID, item.TextureId));
            return true;
        }

        public bool Remove(ItemID itemID, int amount)
        {
            if (GetIemCount(itemID) < amount) return false;

            while (amount > 0)
            {
                var rmv = new List<InventoryItem>();
                foreach (var inventoryItem in Items.Where((item) => item.ItemId == itemID))
                {
                    var inventoryItemCount = inventoryItem.Count;
                    inventoryItem.Remove(inventoryItemCount);
                    amount -= inventoryItemCount;
                    if (inventoryItem.Count <= 0) rmv.Add(inventoryItem);
                }

                foreach (var item in rmv) Items.Remove(item);
            }
            return true;
        }

        private int GetIemCount(ItemID itemID)
        {
            var tmp = 0;
            foreach (var inventoryItem in Items.Where((item) => item.ItemId == itemID)) tmp += inventoryItem.Count;
            return tmp;
        }

        public bool HasSpace(ItemID itemID)
        {
            if (Count < Capacity) return true;
            foreach (var inventoryItem in Items.Where((item) => item.ItemId != itemID))
            {
                if (inventoryItem.Count >= StackCapacity) continue;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var tmp = "";
            foreach (var item in Items) { tmp += $"[{item.ToString()}]"; }
            return tmp;
        }
    }
}
