using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class PartolBehavior : Behavior
    {
        private Vector2? mTarget = null;

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        {
            GetTarget(environment, spaceShip);
            return mTarget switch { null => 0, _ => 0.001 };             
        }

        public override void Execute(SpaceShip spaceShip)
        {
            spaceShip.Velocity = 20;
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mTarget, 0.1f);
            mTarget = (Vector2.Distance(spaceShip.Position, (Vector2)mTarget) < 10000) ? null : mTarget;
        }

        private void GetTarget(List<GameObject> environment, SpaceShip spaceShip)
        {
            if (mTarget is not null) return;
            var targets = environment.OfType<Planet>().ToList();
            if (targets.Count <= 0) return;
            var planet = Utility.Utility.GetRandomElement(targets);
            var angle = Geometry.AngleBetweenVectors(planet.Position, spaceShip.Position);
            mTarget = Geometry.GetPointOnCircle(planet.BoundedBox, angle);
        }
    }
}
