﻿// FighterAttacBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class AttacBehavior : Behavior
    {
        private Vector2? mPosition;
        private IEnumerable<Player> mTargets;

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        {
            mTargets = spaceShip.SensorArray.SortedSpaceShips.OfType<Player>();
            if (!mTargets.Any()) return 0;
            return 0.5;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.WeaponSystem.Fire();
            var target = mTargets.First();
            spaceShip.WeaponSystem.SpaceshipTarget = target;
            if (!spaceShip.SublightEngine.IsTargetReached(spaceShip, mPosition)) return;
            mPosition = ExtendetRandom.NextVectorInCircle(new(target.Position, ExtendetRandom.Random.Next(0, spaceShip.SensorArray.ScanRadius)));
            spaceShip.SublightEngine.SetTarget(spaceShip, mPosition);
        }

    }
}
                                                                    