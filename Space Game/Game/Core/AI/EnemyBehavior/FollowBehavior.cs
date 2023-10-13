// FollowBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FollowBehavior : Behavior
    {
        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        => (spaceShip.WeaponSystem.Target is null) ? 0 : 0.2;

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.SetTarget(spaceShip, spaceShip.WeaponSystem.Target.Position);
        }
    }
}
