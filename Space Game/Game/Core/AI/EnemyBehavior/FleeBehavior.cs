// FleeBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.Utilitys;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FleeBehavior : Behavior
    {
        private readonly double mMinHullLevel;
        private readonly int mFleeDistance;
        private Vector2? mFleeTarget;

        public FleeBehavior(int fleeDistance)
        {
            mMinHullLevel = ExtendetRandom.Random.NextDouble() * 0.62;
            mFleeDistance = fleeDistance;
        }

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip) =>
            (spaceShip.DefenseSystem.HullLevel > mMinHullLevel) ? 0 : 1;

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            if (mFleeTarget is not null) return;
            ExtendetRandom.Random.NextUnitVector(out var vector);
            mFleeTarget = vector * mFleeDistance * 2;
            spaceShip.SublightEngine.SetTarget(spaceShip, mFleeTarget);
        }
    }
}
