// PatrollBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class PatrollBehavior : Behavior
    {
        private Vector2? mPatrolTarget;
        private List<Planet> mPatrolTargets;

        private readonly SpaceShip mSpaceShip;

        public PatrollBehavior(SpaceShip spaceShip) => mSpaceShip = spaceShip;

        public override double GetScore()
        {
            mPatrolTargets = mSpaceShip.SensorSystem.LongRangeScan.OfType<Planet>().ToList();
            return MathF.Min(mPatrolTargets.Count, 1) * 0.1f;
        } 

        public override void Execute()
        {
            // Set velocity to 100%
            mSpaceShip.SublightDrive.SetVelocity(1f);

            // Move to Patrol Target
            switch (mPatrolTarget)
            {
                case null: // Get new Patrol Target

                    if (mPatrolTargets.Count == 0) break;

                    // Get Random Target
                    var randomPlanet = ExtendetRandom.GetRandomElement(mPatrolTargets);
                    var angleBetweenTargetAndShip = Geometry.AngleBetweenVectors(randomPlanet.Position, mSpaceShip.Position);
                    var target = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);
                    mPatrolTarget = target;

                    // Send Ship to Target
                    mSpaceShip.SublightDrive.MoveToTarget(target);
                    break;

                case not null: // Check if Patrol Target is reached
                    if (!mSpaceShip.SublightDrive.IsMoving) 
                        mPatrolTarget = null;
                    break;
            }
        }
    }
}
