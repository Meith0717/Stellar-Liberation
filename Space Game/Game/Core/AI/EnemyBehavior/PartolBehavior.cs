using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.PositionController;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class PartolBehavior : Behavior
    {
        private readonly float mVelocity;

        public PartolBehavior(float velocity)
        {
            mVelocity = velocity;
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip) 
            => (spaceShip.Target is null) ? 1 : 0;

        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {
            spaceShip.Velocity = mVelocity;
            var attacTargets = environment.OfType<Player>();

            switch (attacTargets.Any())
            {
                case true:
                    spaceShip.Target = attacTargets.First();
                    break;

                case false:
                    var patrolTargets = environment.OfType<Planet>().ToList();
                    if (!patrolTargets.Any()) break;
                    var randomPlanet = Utility.Utility.GetRandomElement(patrolTargets);
                    var angleBetweenTargetAndShip = Geometry.AngleBetweenVectors(randomPlanet.Position, spaceShip.Position);
                    var targetPosition = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);

                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, targetPosition, 0.1f);
                    break;
            }
        }
    }
}
