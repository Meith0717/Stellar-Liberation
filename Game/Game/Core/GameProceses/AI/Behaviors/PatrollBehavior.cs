// PatrollBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class PatrollBehavior : Behavior
    {
        private Vector2? mPatrolTarget;

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.5 + spaceShip.DefenseSystem.HullPercentage * 0.5;
            var hasNoAimingShip = !spaceShip.SensorSystem.OpponentsInRannge.Any() ? 1 : 0;

            var score = shielHhullScore * hasNoAimingShip;
            return score;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            spaceShip.SublightDrive.SetVelocity(.5f);

            var aimingShip = spaceShip.SensorSystem.GetAimingShip(spaceShip.Position);
            spaceShip.WeaponSystem.AimShip(aimingShip);


            switch (mPatrolTarget)
            {
                case null:
                    // Get Planets in Radius
                    var patrolTargets = spaceShip.SensorSystem.LongRangeScan.OfType<Planet>().ToList();
                    if (!patrolTargets.Any()) break;

                    // Get Random Target
                    var randomPlanet = ExtendetRandom.GetRandomElement(patrolTargets);
                    var angleBetweenTargetAndShip = Geometry.AngleBetweenVectors(randomPlanet.Position, spaceShip.Position);
                    var target = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);
                    mPatrolTarget = target;

                    // Send Ship to Target
                    spaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(target - spaceShip.Position));
                    break;
                case not null:
                    // Check if Target is Reached
                    if (Vector2.Distance((Vector2)mPatrolTarget, spaceShip.Position) < 10000) mPatrolTarget = null;
                    break;
            }
        }

        public override void Reset(SpaceShip spaceShip) => mPatrolTarget = null;
    }
}
