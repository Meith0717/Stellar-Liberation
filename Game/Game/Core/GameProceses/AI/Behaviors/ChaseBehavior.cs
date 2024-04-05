// ChaseBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.SpaceCrafts;
using System;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class ChaseBehavior : Behavior
    {
        private Flagship mPatrolTarget;
        private readonly Flagship mSpaceship;

        public ChaseBehavior(Flagship spaceShip)
        {
            mSpaceship = spaceShip;
        }

        public override double GetScore()
        {
            var shialdHullScore = mSpaceship.Defense.HullPercentage * 0.95 + mSpaceship.Defense.ShieldPercentage * 0.05;
            var targets = mSpaceship.Sensors.Opponents;
            var targetScore = MathF.Min(targets.Count, 1);
            return targetScore * shialdHullScore * .1f;
        }

        public override void Execute()
        {

            // Move to Patrol Target
            switch (mPatrolTarget)
            {
                case null: // Get new Patrol Target
                    var targets = mSpaceship.Sensors.Opponents;
                    mPatrolTarget = targets.First();
                    mSpaceship.ImpulseDrive.MoveToTarget(mPatrolTarget.Position);
                    break;

                case not null: // Check if Patrol Target is reached
                    if (mSpaceship.ImpulseDrive.IsMoving
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
