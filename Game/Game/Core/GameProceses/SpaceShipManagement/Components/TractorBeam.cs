﻿// TractorBeam.cs 
// Copyright (c) 2023 Thierry Meiers 
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
    public class TractorBeam
    {
        [JsonProperty] private float mPullRadius;
        [JsonIgnore] private List<Item> mItemsInRange = new();

        public TractorBeam(float pullRadius) => mPullRadius = pullRadius;

        public void Pull(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            var inventory = scene.GameState.Inventory;

            mItemsInRange.Clear();
            var position = spaceShip.Position;

            mItemsInRange = scene.SpatialHashing.GetObjectsInRadius<Item>(position, (int)mPullRadius)
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

        public void Draw(SpaceShip space) { foreach (var item in mItemsInRange) TextureManager.Instance.DrawLine(space.Position, item.Position, Color.Purple, 4, space.TextureDepth - 1); }
    }
}
