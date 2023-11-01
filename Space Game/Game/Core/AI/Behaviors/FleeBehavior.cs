// FleeBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;
using System.Linq;


namespace CelestialOdyssey.Game.Core.AI.Behaviors
{
    public class FleeBehavior : Behavior
    {
        private Vector2? mFleePosition;

        public override double GetScore(SensorArray environment, SpaceShip spaceShip)
        {
            var shieldScore = spaceShip.DefenseSystem.ShildLevel * 0.01;
            var hullScore = spaceShip.DefenseSystem.HullLevel * 0.99;
            var target = environment.AimingShip;
            if (target is null) 
            {
                System.Diagnostics.Debug.WriteLine($"FleeScore: {0}");
                return 0; 
            }
            var targetShieldScore = target.DefenseSystem.ShildLevel * 0.1;
            var targetHullScore = target.DefenseSystem.HullLevel * 0.9;

            var score = ((targetShieldScore + targetHullScore) * 0.5) + ((1 - (shieldScore + hullScore)));
            System.Diagnostics.Debug.WriteLine($"FleeScore: {score}");
            return score;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            if (mFleePosition is null) mFleePosition = environment.AstronomicalObjects.Last().Position;
            spaceShip.SublightEngine.MoveToPosition((Vector2)mFleePosition);
        }

        public override void Reset(SpaceShip spaceShip) => mFleePosition = null;
    }
}
