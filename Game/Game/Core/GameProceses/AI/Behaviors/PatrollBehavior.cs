// PatrollBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class PatrollBehavior : Behavior
    {
        private SpaceShip mPatrolTarget;
        private List<SpaceShip> mPatrolTargets;

        private readonly SpaceShip mSpaceShip;

        public PatrollBehavior(SpaceShip spaceShip)
        {
            mSpaceShip = spaceShip;
        }

        public override double GetScore()
        {
            mPatrolTargets = mSpaceShip.SensorSystem.Opponents;
            return MathF.Min(mPatrolTargets.Count, 1) * 0.1f;
        }

        public override void Execute()
        {
            mSpaceShip.PhaserCannaons.StopFire();

            // Move to Patrol Target
            switch (mPatrolTarget)
            {
                case null: // Get new Patrol Target
                    mSpaceShip.SublightDrive.SetVelocity(0);
                    if (mPatrolTargets.Count == 0) break;
                    mPatrolTarget = mPatrolTargets.First();
                    mSpaceShip.SublightDrive.FollowSpaceShip(mPatrolTarget);
                    break;

                case not null: // Check if Patrol Target is reached
                    mSpaceShip.SublightDrive.SetVelocity(1f);
                    if (mSpaceShip.SublightDrive.IsMoving) break;
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
