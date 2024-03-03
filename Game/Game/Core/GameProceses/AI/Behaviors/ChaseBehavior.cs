// PatrollBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class ChaseBehavior : Behavior
    {
        private SpaceShip mPatrolTarget;
        private readonly SpaceShip mSpaceShip;

        public ChaseBehavior(SpaceShip spaceShip)
        {
            mSpaceShip = spaceShip;
        }

        public override double GetScore()
        {
            var shialdHullScore = mSpaceShip.DefenseSystem.HullPercentage * 0.95 + mSpaceShip.DefenseSystem.ShieldPercentage * 0.05;
            var targets = mSpaceShip.SensorSystem.Opponents;
            var targetScore = MathF.Min(targets.Count, 1);
            return targetScore * shialdHullScore * .1f;
        }

        public override void Execute()
        {
            mSpaceShip.PhaserCannaons.StopFire();

            // Move to Patrol Target
            switch (mPatrolTarget)
            {
                case null: // Get new Patrol Target
                    var targets = mSpaceShip.SensorSystem.Opponents;
                    mSpaceShip.SublightDrive.FollowSpaceShip(mPatrolTarget = targets.First());
                    mSpaceShip.SublightDrive.SetVelocity(.2f);
                    break;

                case not null: // Check if Patrol Target is reached
                    mSpaceShip.SublightDrive.SetVelocity(1f);
                    if (mSpaceShip.SublightDrive.IsMoving 
                        && !mPatrolTarget.IsDisposed) break;
                    mPatrolTarget = null;
                    break;
            }
        }

        public override void Recet()
        {
            mPatrolTarget = null;
        }
    }
}
