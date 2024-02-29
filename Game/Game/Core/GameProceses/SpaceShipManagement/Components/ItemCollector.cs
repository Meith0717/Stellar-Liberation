// TractorBeam.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components
{
    [Serializable]
    public class ItemCollector
    {
        [JsonIgnore] private List<Item> mItemsInRange = new();

        public void Collect(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            var inventory = spaceShip.Inventory;

            mItemsInRange.Clear();
            var position = spaceShip.Position;

            mItemsInRange = scene.SpatialHashing.GetObjectsInRadius<Item>(position, spaceShip.BoundedBox.Radius)
                .Where((item) => inventory.HasSpace(item.ItemID))
                .ToList();

            foreach (var item in mItemsInRange)
            {
                item.Pull(position);
                if (!ContinuousCollisionDetection.HasCollide(gameTime, item, spaceShip, out _)) continue;
                inventory.Add(item);
                System.Diagnostics.Debug.WriteLine(inventory.ToString());
            }
        }
    }
}
