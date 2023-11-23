// FleeBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;
using System.Linq;
using StellarLiberation.Game.Core.LayerManagement;

namespace StellarLiberation.Game.Core.AI.Behaviors
{
    public class FleeBehavior : Behavior
    {
        private Vector2? mFleePosition;

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.01 + spaceShip.DefenseSystem.HullPercentage * 0.99;

            var target = spaceShip.SensorArray.AimingShip;
            if (target is null) return 0; 
            var targetShielHhullScore = target.DefenseSystem.ShieldPercentage * 0.1 + target.DefenseSystem.HullPercentage * 0.9;

            var score = (targetShielHhullScore / 2) * ((1 - shielHhullScore) * 2);
            return score;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            if (mFleePosition is null) mFleePosition = spaceShip.SensorArray.AstronomicalObjects.Last().Position;
            spaceShip.SublightEngine.MoveToPosition((Vector2)mFleePosition);
        }

        public override void Reset(SpaceShip spaceShip) => mFleePosition = null;
    }
}
