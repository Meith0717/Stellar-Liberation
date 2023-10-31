// FighterAttacBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.Utilitys;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class AttacBehavior : Behavior
    {
        private bool mReorienting;
        private Vector2? mReorientingPosition;

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
            var distanceToTarget = Vector2.Distance(spaceShip.Position, environment.AimingShip.Position);
            var aimingShip = spaceShip.SensorArray.AimingShip; 

            switch (mReorienting)
            {
                case true:
                    spaceShip.WeaponSystem.StopFire();

                    // Check for breaking reorientation
                    if (!spaceShip.SublightEngine.IsMoving)
                    {
                        mReorienting = false;
                        // Set course to attac target
                        spaceShip.SublightEngine.SetTarget(spaceShip, aimingShip.Position);
                        break;
                    }

                    // Check if reorientation Position has been reached 
                    if (!spaceShip.SublightEngine.IsTargetReached(spaceShip, mReorientingPosition)) break;
                    // Get new reorientation Position
                    GetReorientingPosition(environment, spaceShip);
                    // Set course to new reorientation Position
                    spaceShip.SublightEngine.SetTarget(spaceShip, mReorientingPosition);
                    break;

                case false:
                    mReorientingPosition = null;

                    // Check if a reorientation is needet 
                    if (distanceToTarget < 1000)
                    {
                        mReorienting = true;
                        // Get reorientation Position
                        GetReorientingPosition(environment, spaceShip);
                        // Set course to reorientation Position
                        spaceShip.SublightEngine.SetTarget(spaceShip, mReorientingPosition);
                        break;
                    }

                    // Set course to attac target
                    spaceShip.SublightEngine.SetTarget(spaceShip, environment.AimingShip.Position);
                    spaceShip.WeaponSystem.Fire();
                    break;
            }
        }

        private void GetReorientingPosition(SensorArray environment, SpaceShip spaceShip)
        {
            var targetPos = environment.AimingShip.Position;
            mReorientingPosition = ExtendetRandom.NextVectorOnBorder(new(targetPos, 5000));
        }

        public override void Reset(SpaceShip spaceShip)
        {
            mReorienting = false;
            mReorientingPosition = null;
            spaceShip.WeaponSystem.StopFire();
        }
    }
}
                                                                    