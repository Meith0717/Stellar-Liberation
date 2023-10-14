// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Collision_Detection;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ItemManagement
{
    [Serializable]
    public class Inventory
    {
        [JsonProperty] private readonly Dictionary<Type, int> mItems = new();

        public void Update(SpaceShip spaceShip, Scene scene)
        {
            var position = spaceShip.Position;
            var itemsInRange = scene.GetObjectsInRadius<Item>(position, 1000);
            var collected = false;
            foreach (var item in itemsInRange)
            {
                item.Pull(position);
                if (!ContinuousCollisionDetection.HasCollide(item, spaceShip, out _)) continue;
                spaceShip.ActualPlanetSystem.ItemManager.RemoveItem(scene, item);
                Collect(item);
                collected = true;
            }
            if (collected) SoundManager.Instance.PlaySound(ContentRegistry.collect, 1f);
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
    }
}
