// ItemCollector.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components
{
    public class ItemCollector
    {
        private List<Container> mContainerInRange = new();

        public void Collect(GameTime gameTime, Spaceship spaceShip, PlanetsystemState planetsystemState)
        {
            var inventory = spaceShip.Inventory;

            mContainerInRange.Clear();
            var position = spaceShip.Position;

            mContainerInRange = planetsystemState.SpatialHashing.GetObjectsInRadius<Container>(position, spaceShip.BoundedBox.Radius);

            foreach (var container in mContainerInRange)
            {
                container.IsDisposed = true;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, container, spaceShip, out _)) continue;
                foreach (var item in container.Items)
                {
                    if (!inventory.HasSpace(item)) continue;
                    inventory.Add(item);
                }
                planetsystemState.StereoSounds.Enqueue(new(spaceShip.Position, SoundEffectRegistries.collect));
            }
        }
    }
}
