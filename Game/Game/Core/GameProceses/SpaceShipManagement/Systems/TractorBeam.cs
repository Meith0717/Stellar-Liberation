// TractorBeam.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems
{
    [Serializable]
    public class TractorBeam
    {
        [JsonProperty] private float mPullRadius;
        [JsonIgnore] private List<Item> mItemsInRange = new();

        public TractorBeam(float pullRadius) => mPullRadius = pullRadius;

        public void Pull(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            var inventory = scene.GameLayer.Inventory;

            mItemsInRange.Clear();
            if (inventory.Count == inventory.Capacity) return;
            var position = spaceShip.Position;
            mItemsInRange = scene.SpatialHashing.GetObjectsInRadius<Item>(position, (int)mPullRadius);
            foreach (var item in mItemsInRange)
            {
                item.Pull(position);
                if (ContinuousCollisionDetection.HasCollide(gameTime, item, spaceShip, out _)) inventory.Collect(item);
            }
        }

        public void Draw(SpaceShip space) { foreach (var item in mItemsInRange) TextureManager.Instance.DrawLine(space.Position, item.Position, Color.Purple, 4, space.TextureDepth - 1); }
    }
}
