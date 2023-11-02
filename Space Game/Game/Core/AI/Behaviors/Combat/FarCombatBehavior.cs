// FarCombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.AI.Behaviors.Combat
{
    internal class FarCombatBehavior : CombatBehavior
    {
        public FarCombatBehavior(float attacDistance) : base(attacDistance) { }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.ClearTarget();
            spaceShip.WeaponSystem.StopFire();
            spaceShip.WeaponSystem.StopFire();
            if (mDistance < mAttackDistance) spaceShip.WeaponSystem.Fire();
        }

        public override void Reset(SpaceShip spaceShip) => spaceShip.WeaponSystem.StopFire();
    }
}
