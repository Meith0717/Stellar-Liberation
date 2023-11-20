﻿// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.GameObjects;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.ItemManagement
{
    [Serializable]
    public class Inventory { 

        [JsonProperty] public int Count { get; private set; }
        [JsonProperty] public int Capacity { get; private set; }

        [JsonProperty] private readonly Dictionary<ItemID, int> Items = new();
        [JsonProperty] private float mPullRadius;
        [JsonIgnore] private List<Item> mItemsInRange = new();

        public Inventory(int capacity, int PullRadius) { mPullRadius = PullRadius; Capacity = capacity; }

        public void Update(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            mItemsInRange.Clear();
            if (Count >= Capacity) return;
            var position = spaceShip.Position;
            mItemsInRange = scene.SpatialHashing.GetObjectsInRadius<Item>(position, (int)mPullRadius);
            foreach (var item in mItemsInRange)
            {
                item.Pull(position);
                if (ContinuousCollisionDetection.HasCollide(gameTime, item, spaceShip, out _)) Collect(item, scene);
            }
        }

        private void Collect(Item item, Scene scene)
        {
            SoundManager.Instance.PlaySound(SoundRegistries.collect, 1f);

            // Add Item in Inventory
            if (!Items.TryGetValue(item.ItemID, out _)) Items[item.ItemID] = 0;
            Items[item.ItemID]++;

            // Some Other stuff
            scene.GameLayer.ItemManager.RemoveItem(scene, item);
            Count ++;
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

        public void Draw(SpaceShip space) { foreach (var item in mItemsInRange) TextureManager.Instance.DrawLine(space.Position, item.Position, Color.Purple, 4, space.TextureDepth - 1); }
    }
}
