// FleeBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class FleeBehavior : Behavior
    {
        private Vector2? mFleePosition;

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.01 + spaceShip.DefenseSystem.HullPercentage * 0.99;

            var target = spaceShip.WeaponSystem.AimingShip;
            if (target is null) return 0;
            var targetShielHhullScore = target.DefenseSystem.ShieldPercentage * 0.1 + target.DefenseSystem.HullPercentage * 0.9;

            var score = targetShielHhullScore / 2 * ((1 - shielHhullScore) * 2);
            return score;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            spaceShip.SublightDrive.SetVelocity(1f);
            mFleePosition ??= spaceShip.SensorSystem.LongRangeScan.OfType<Planet>().Last().Position;
            spaceShip.SublightDrive.MoveInDirection(Vector2.Normalize((Vector2)mFleePosition - spaceShip.Position));
        }

        public override void Reset(SpaceShip spaceShip) => mFleePosition = null;
    }
}
