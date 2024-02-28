// PatrollBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
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

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            mPatrolTargets = spaceShip.SensorSystem.LongRangeScan.OfType<Planet>().ToList();
            return MathF.Min(mPatrolTargets.Count, 1) * 0.1f;
        } 

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            // Set velocity to 100%
            spaceShip.SublightDrive.SetVelocity(1f);

            // Move to Patrol Target
            switch (mPatrolTarget)
            {
                case null: // Get new Patrol Target

                    if (mPatrolTargets.Count == 0) break;

                    // Get Random Target
                    var randomPlanet = ExtendetRandom.GetRandomElement(mPatrolTargets);
                    var angleBetweenTargetAndShip = Geometry.AngleBetweenVectors(randomPlanet.Position, spaceShip.Position);
                    var target = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);
                    mPatrolTarget = target;

                    // Send Ship to Target
                    spaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(target - spaceShip.Position));
                    break;
                case not null: // Check if Patrol Target is reached
                    if (Vector2.Distance((Vector2)mPatrolTarget, spaceShip.Position) < 10000) mPatrolTarget = null;
                    break;
            }
        }

        public override void Reset(SpaceShip spaceShip) => mPatrolTarget = null;
    }
}
