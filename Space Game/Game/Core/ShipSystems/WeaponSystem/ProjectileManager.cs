// ProjectileManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class ProjectileManager
    {
        public List<Projectile> Projectiles { get; private set; } = new();

        public void AddProjectiel(Projectile projectile)
        {
            Projectiles.Add(projectile);
        }

        public void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            List<Projectile> deleteList = new();
            foreach (var projectile in Projectiles)
            {
                if (projectile.LiveTime16 > projectile.DeleteTime)
                {
                    deleteList.Add(projectile);
                    continue;
                }
                projectile.Update(gameTime, inputState, sceneManagerLayer, scene);
            }

            foreach (var projectile in deleteList)
            {
                projectile.RemoveFromSpatialHashing(scene);
                Projectiles.Remove(projectile);
            }

        }
    }
}
