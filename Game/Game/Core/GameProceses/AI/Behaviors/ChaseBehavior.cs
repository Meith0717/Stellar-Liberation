// PatrollBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class ChaseBehavior : Behavior
    {
        private Spaceship mPatrolTarget;
        private readonly Spaceship mSpaceship;

        public ChaseBehavior(Spaceship spaceShip)
        {
            mSpaceship = spaceShip;
        }

        public override double GetScore()
        {
            var shialdHullScore = mSpaceship.DefenseSystem.HullPercentage * 0.95 + mSpaceship.DefenseSystem.ShieldPercentage * 0.05;
            var targets = mSpaceship.SensorSystem.Opponents;
            var targetScore = MathF.Min(targets.Count, 1);
            return targetScore * shialdHullScore * .1f;
        }

        public override void Execute()
        {
            mSpaceship.PhaserCannaons.StopFire();

            // Move to Patrol Target
            switch (mPatrolTarget)
            {
                case null: // Get new Patrol Target
                    var targets = mSpaceship.SensorSystem.Opponents;
                    mSpaceship.SublightDrive.FollowSpaceship(mPatrolTarget = targets.First());
                    mSpaceship.SublightDrive.SetVelocity(.2f);
                    break;

                case not null: // Check if Patrol Target is reached
                    mSpaceship.SublightDrive.SetVelocity(1f);
                    if (mSpaceship.SublightDrive.IsMoving 
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
