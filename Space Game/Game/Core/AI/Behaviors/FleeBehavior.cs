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
            var shielHhullScore = spaceShip.DefenseSystem.ShildLevel * 0.01 + spaceShip.DefenseSystem.HullLevel * 0.99;

            var target = environment.AimingShip;
            if (target is null) return 0; 
            var targetShielHhullScore = target.DefenseSystem.ShildLevel * 0.1 + target.DefenseSystem.HullLevel * 0.9;

            var score = (targetShielHhullScore / 2) * ((1 - shielHhullScore) * 2);
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
