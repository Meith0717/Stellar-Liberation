// ProjectileManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    public class ProjectileManager : GameObjectManager
    {
        public void AddProjectiel(Projectile projectile)
        {
            AddObj(projectile);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            base.Update(gameTime, inputState, gameLayer, scene);
        }
    }
}
