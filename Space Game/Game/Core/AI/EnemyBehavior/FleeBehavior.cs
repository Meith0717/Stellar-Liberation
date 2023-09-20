using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FleeBehavior : Behavior
    {
        private float mFleeVelocity;

        public FleeBehavior(float velocity)
        {
            this.mFleeVelocity = velocity;
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        {
            return 1;
        }

        public override void Execute(SpaceShip spaceShip)
        {
            try
            {
                var fleeTarget = spaceShip.SensorArray.SortedObjectsInRange.OfType<Planet>().Last();
                spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, fleeTarget.Position, 0.05f);
            } catch
            { ; }
        }
    }
}
