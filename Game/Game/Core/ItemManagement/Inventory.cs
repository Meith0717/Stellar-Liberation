// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.ItemManagement
{
    [Serializable]
    public class Inventory { 

        [JsonProperty] private int mPullRadius;
        [JsonProperty] private readonly Dictionary<Type, int> mItems = new();
        [JsonIgnore] private List<Item> mItemsInRange;

        public Inventory(int PullRadius) { mPullRadius = PullRadius; }

        public void Update(SpaceShip spaceShip, Scene scene)
        {
            var position = spaceShip.Position;
            mItemsInRange = scene.GetObjectsInRadius<Item>(position, mPullRadius);
            var collected = false;
            foreach (var item in mItemsInRange)
            {
                item.Pull(position);
                if (!ContinuousCollisionDetection.HasCollide(item, spaceShip, out _)) continue;
                scene.GameLayer.ItemManager.RemoveItem(scene, item);
                Collect(item);
                collected = true;
            }
            if (collected) SoundManager.Instance.PlaySound(SoundRegistries.collect, 1f);
        }

        public void Collect(Item item)
        {
            if (!mItems.TryGetValue(item.GetType(), out _)) mItems[item.GetType()] = 0;
            mItems[item.GetType()]++;
        }

        public bool Drop(Item item)
        {
            if (!mItems.TryGetValue(item.GetType(), out _)) return false;
            mItems[item.GetType()]--;
            if (mItems[item.GetType()] == 0) mItems.Remove(item.GetType());
            return true;
        }

        public void Draw(SpaceShip space)
        {
            foreach (var item in mItemsInRange) TextureManager.Instance.DrawLine(space.Position, item.Position, Color.Purple, 4, space.TextureDepth - 1);
        }
    }
}
