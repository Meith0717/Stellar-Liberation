using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using System.Linq;
using Microsoft.Xna.Framework;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.PositionController;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FleeBehavior : Behavior
    {
        private readonly double mMinHullLevel;
        private readonly float mVelocity;
        private Vector2? mFleeTarget;

        public FleeBehavior(float velocity)
        {
            mMinHullLevel = 0.5; //Utility.Utility.Random.NextDouble() * 0.5;
            mVelocity = velocity;
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        {
            if (spaceShip.DefenseSystem.HullLevel > mMinHullLevel) return 0;
            if (mFleeTarget is not null)
            {
                if (Vector2.Distance((Vector2)mFleeTarget, spaceShip.Position) < 1000) return 0;
                return 1;
            }
            var fleeTargets = environment.OfType<Planet>();
            if (!fleeTargets.Any()) return 0;
            mFleeTarget = fleeTargets.Last().Position;
            spaceShip.Target = null;
            return 1;            
        }

        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {
            spaceShip.Velocity = mVelocity;
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mFleeTarget, 0.1f);
        }
    }
}
