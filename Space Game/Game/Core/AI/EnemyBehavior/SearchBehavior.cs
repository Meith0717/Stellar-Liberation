// PartolBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class SearchBehavior : Behavior
    {
        private Vector2? mPatrolTarget;

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        {
            var shieldLevel = spaceShip.DefenseSystem.ShildLevel;
            var hullLevel = spaceShip.DefenseSystem.HullLevel;
            var hasNoTarget = environment.AimingShip is null ? 1 : 0;
            return (shieldLevel + hullLevel) * hasNoTarget * 100;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            switch (mPatrolTarget)
            {
                case null:
                    // Get Planets in Radius
                    var patrolTargets = environment.AstronomicalObjects.OfType<Planet>().ToList();
                    if (!patrolTargets.Any()) break;

                    // Get Random Target
                    var randomPlanet = ExtendetRandom.GetRandomElement(patrolTargets);
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

        public override void Reset(SpaceShip spaceShip) => mPatrolTarget = null;
    }
}
