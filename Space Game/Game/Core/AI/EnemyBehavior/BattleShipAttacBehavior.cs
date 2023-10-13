// BattleShipAttacBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class BattleShipAttacBehavior : Behavior
    {
        private readonly float mAttacDistance;

        public BattleShipAttacBehavior(int attacDistance)
        {
            mAttacDistance = attacDistance;
        }

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        {
            if (spaceShip.WeaponSystem.Target is null) return 0;
            var distanceToTarget = Vector2.Distance(spaceShip.WeaponSystem.Target.Position, spaceShip.Position);
            if (distanceToTarget > mAttacDistance) return 0;
            return 0.5;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.WeaponSystem.Fire(spaceShip.ActualPlanetSystem.ProjectileManager, spaceShip);
        }

    }
}
