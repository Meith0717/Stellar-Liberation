// PatrollBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.Behaviors
{
    public class PatrollBehavior : Behavior
    {
        private Vector2? mPatrolTarget;

        public override double GetScore(SensorArray environment, SpaceShip spaceShip)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShildLevel * 0.5 + spaceShip.DefenseSystem.HullLevel * 0.5;
            var hasNoAimingShip = environment.AimingShip is null ? 1 : 0;

            var score = shielHhullScore * hasNoAimingShip;
            return score;
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
                    var target = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);
                    mPatrolTarget = target;

                    // Send Ship to Target
                    spaceShip.SublightEngine.MoveToPosition(target);
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
