// ProjectileManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Layers;
using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    public class ProjectileManager : GameObjectManager
    {
        public void AddProjectiel(Projectile projectile)
        {
            AddObj(projectile);
        }
    }
}
