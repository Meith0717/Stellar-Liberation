// FighterAttacBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.GameObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class AttacBehavior1 : Behavior
    {
        private Vector2? mPosition;
        private IEnumerable<Enemy> mTargets;

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        {
            mTargets = spaceShip.SensorArray.SortedSpaceShips.OfType<Enemy>();
            if (!mTargets.Any()) return 0;
            return 0.5;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.WeaponSystem.Fire();
            var target = mTargets.First();
            spaceShip.WeaponSystem.SpaceshipTarget = target;
            if (!spaceShip.SublightEngine.IsTargetReached(spaceShip, mPosition)) return;
            mPosition = Utility.Utility.GetRandomVector2(new(target.Position, Utility.Utility.Random.Next(0, spaceShip.SensorArray.ScanRadius)));
            spaceShip.SublightEngine.SetTarget(spaceShip, mPosition);
        }

    }
}
