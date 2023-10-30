// FighterAttacBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class AttacBehavior : Behavior
    {
        private Vector2? mPosition;

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        {
            var shieldScore = spaceShip.DefenseSystem.ShildLevel * 0.4;
            var hullScore = spaceShip.DefenseSystem.HullLevel * 0.6;
            var target = environment.AimingShip;
            if (target is null) return 0;
            var targetShieldScore = target.DefenseSystem.ShildLevel * 0.1;
            var targetHullScore = target.DefenseSystem.HullLevel * 0.9;

            var score = shieldScore + hullScore + ((1 - (targetShieldScore + targetHullScore)) * 10);
            System.Diagnostics.Debug.WriteLine(score);
            return score;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.WeaponSystem.Fire();
            switch (mPosition)
            {
                case null:
                    spaceShip.SublightEngine.SetTarget(spaceShip, mPosition);
                    break;
                case not null:
                    if (!spaceShip.SublightEngine.IsMoving) mPosition = null;
                    break;
            }
        }

        public override void Reset(SpaceShip spaceShip)
        {
            mPosition = null;
            spaceShip.WeaponSystem.StopFire();
        }
    }
}
                                                                    