// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.Items;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.ItemManagement
{
    [Serializable]
    public class Inventory
    {

        [JsonProperty] public int Count { get; private set; }
        [JsonProperty] public int Capacity { get; private set; }

        [JsonProperty] private readonly Dictionary<ItemID, int> Items = new();

        public Inventory(int capacity) => Capacity = capacity;

        public void Collect(Item item, Scene scene)
        {
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.collect);

            // Add Item in Inventory
            if (!Items.TryGetValue(item.ItemID, out _)) Items[item.ItemID] = 0;
            Items[item.ItemID]++;

            // Some Other stuff
            scene.GameLayer.CurrentSystem.GameObjects.RemobeObj(item);
            Count++;
        }

        public bool RemoveItemAmount(ItemID itemID, int amount)
        {
            if (!Items.TryGetValue(itemID, out var _)) return false;
            if (Items[itemID] < amount) return false;
            Items[itemID] -= amount;
            if (Items[itemID] == 0) Items.Remove(itemID);
            return true;
        }

        public int GetIDAmount(ItemID itemID)
        {
            if (!Items.TryGetValue(itemID, out var count)) return 0;
            return count;
        }
    }
}
