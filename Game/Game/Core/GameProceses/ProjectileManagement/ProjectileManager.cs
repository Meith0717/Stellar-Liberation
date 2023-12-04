// ProjectileManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;

namespace StellarLiberation.Game.Core.GameProceses.ProjectileManagement
{
    public class ProjectileManager : GameObjectManager
    {
        public void AddProjectiel(Projectile projectile)
        {
            AddObj(projectile);
        }
    }
}
