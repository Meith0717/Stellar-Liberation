using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;
using System;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using System.Linq;
using Microsoft.Xna.Framework;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FleeBehavior : Behavior
    {
        private double mMinHullLevel;
        private Vector2? mFleeTarget;

        public FleeBehavior()
        {
            mMinHullLevel = 0.5; //Utility.Utility.Random.NextDouble() * 0.5;
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
            spaceShip.Velocity = 60;
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mFleeTarget, 0.1f);
        }
    }
}
