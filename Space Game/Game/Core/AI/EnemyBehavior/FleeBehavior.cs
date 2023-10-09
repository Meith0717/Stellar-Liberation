using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using CelestialOdyssey.Game.Core.ShipSystems;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FleeBehavior : Behavior
    {
        private readonly double mMinHullLevel;
        private readonly int mFleeDistance; 
        private Vector2? mFleeTarget;

        public FleeBehavior(int fleeDistance)
        {
            mMinHullLevel = Utility.Utility.Random.NextDouble() * 0.62;
            mFleeDistance = fleeDistance;
        }

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip) =>
            (spaceShip.DefenseSystem.HullLevel > mMinHullLevel) ? 0 : 1;            

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            if (mFleeTarget is not null) return;
            Utility.Utility.Random.NextUnitVector(out var vector);
            mFleeTarget = vector * mFleeDistance * 2;
            spaceShip.SublightEngine.SetTarget(spaceShip, mFleeTarget);
        }
    }
}
