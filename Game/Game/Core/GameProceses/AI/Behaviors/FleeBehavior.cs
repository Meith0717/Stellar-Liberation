// FleeBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class FleeBehavior : Behavior
    {
        private readonly SpaceShip mSpaceShip;

        public FleeBehavior(SpaceShip spaceShip) 
            => mSpaceShip = spaceShip;

        public override double GetScore()
        {
            var opponents = mSpaceShip.SensorSystem.OpponentsInRannge;
            if (opponents.Count <= 0) return 0;
            var opponentsInRageScore = 2 - (1 / (0.1d * opponents.Count + 1));

            var hullScore = 1 - mSpaceShip.DefenseSystem.HullPercentage;
            return hullScore * opponentsInRageScore;
        }

        public override void Execute()
        {
            mSpaceShip.PhaserCannaons.StopFire();
            var opponents = mSpaceShip.SensorSystem.OpponentsInRannge;
            var dir = Vector2.Zero;
            foreach (var opponent in opponents) dir -= Vector2.Subtract(opponent.Position, mSpaceShip.Position);
            dir.Normalize();
            mSpaceShip.SublightDrive.MoveInDirection(dir);
            mSpaceShip.SublightDrive.SetVelocity(1);
        }

        public override void Recet(){ }
    }
}
