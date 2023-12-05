// SpaceShipManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using StellarLiberation.Game.GameObjects.SpaceShips.Enemys;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement
{
    public class SpaceShipManager : GameObjectManager
    {
        public void AddRange(List<SpaceShip> spaceShips) => base.AddRange(spaceShips);

        public void Spawn(Vector2 position, EnemyId type) => AddObj(EnemyFactory.Get(type, position));
    }
}
