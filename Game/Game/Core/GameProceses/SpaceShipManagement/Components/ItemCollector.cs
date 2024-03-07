// TractorBeam.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.GameObjects.Recources;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components
{
    public class ItemCollector
    {
        private List<Container> mContainerInRange = new();

        public void Collect(GameTime gameTime, Spaceship spaceShip, GameLayer scene)
        {
            var inventory = spaceShip.Inventory;

            mContainerInRange.Clear();
            var position = spaceShip.Position;

            mContainerInRange = scene.SpatialHashing.GetObjectsInRadius<Container>(position, spaceShip.BoundedBox.Radius);

            foreach (var container in mContainerInRange)
            {
                container.IsDisposed = true;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, container, spaceShip, out _)) continue;
                foreach (var item in  container.Items)
                {
                    if (!inventory.HasSpace(item)) continue;
                    inventory.Add(item);
                }
                SoundEffectSystem.PlaySound(SoundEffectRegistries.collect, scene.Camera2D, spaceShip.Position);
            }
        }
    }
}
