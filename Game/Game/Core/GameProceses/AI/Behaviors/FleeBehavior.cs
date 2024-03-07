// FleeBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class FleeBehavior : Behavior
    {
        private readonly Spaceship mSpaceship;

        public FleeBehavior(Spaceship spaceShip) 
            => mSpaceship = spaceShip;

        public override double GetScore()
        {
            var shieldHullScore = 1 - (mSpaceship.DefenseSystem.HullPercentage * 0.85 + mSpaceship.DefenseSystem.ShieldPercentage * 0.15);

            var opponents = mSpaceship.SensorSystem.OpponentsInRannge;
            var opponentShieldHullScore = 0d;
            foreach (var opponent in opponents)
            {
                opponentShieldHullScore += opponent.DefenseSystem.HullPercentage * 0.85 + mSpaceship.DefenseSystem.ShieldPercentage * 0.15;
            }
            opponentShieldHullScore = opponents.Count == 0 ? 0 : opponentShieldHullScore / opponents.Count;
            var opponentsScore = 1 - (1 / (.5 * opponentShieldHullScore + 1));

            return shieldHullScore * opponentsScore;
        }

        public override void Execute()
        {
            mSpaceship.PhaserCannaons.StopFire();
            var opponents = mSpaceship.SensorSystem.OpponentsInRannge;
            var dir = Vector2.Zero;
            foreach (var opponent in opponents) dir -= Vector2.Subtract(opponent.Position, mSpaceship.Position);
            dir.Normalize();
            mSpaceship.SublightDrive.SetVelocity(1);
            mSpaceship.SublightDrive.MoveInDirection(dir);
        }

        public override void Recet(){ }
    }
}
