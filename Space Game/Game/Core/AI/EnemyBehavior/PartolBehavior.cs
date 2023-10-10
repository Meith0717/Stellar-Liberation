using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class PartolBehavior : Behavior
    {
        private Vector2? mPatrolTarget;

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip) 
            => (spaceShip.WeaponSystem.Target is null) ? 1 : 0.1;

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            var targets = environment.SortedSpaceShips.OfType<Player>();
            if (targets.Any()) spaceShip.WeaponSystem.Target = targets.First();

            switch (mPatrolTarget)
            {
                case null:
                    if (Utility.Utility.Random.NextDouble() > 1) break;

                    // Get Planets in Radius
                    var patrolTargets = environment.AstronomicalObjects.OfType<Planet>().ToList();
                    if (!patrolTargets.Any()) break;

                    // Get Random Target
                    var randomPlanet = Utility.Utility.GetRandomElement(patrolTargets);
                    var angleBetweenTargetAndShip = Geometry.AngleBetweenVectors(randomPlanet.Position, spaceShip.Position);
                    mPatrolTarget = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);

                    // Send Ship to Target
                    spaceShip.SublightEngine.SetTarget(spaceShip, mPatrolTarget);
                    break;
                case not null:

                    // Check if Target is Reached
                    if (Vector2.Distance((Vector2)mPatrolTarget, spaceShip.Position) < 10000) mPatrolTarget = null;
                    break;
            }
        }
    }
}
