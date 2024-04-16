// ChaseBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class ChaseBehavior : Behavior
    {
        private Spacecraft mPatrolTarget;
        private readonly Battleship mSpaceship;

        public ChaseBehavior(Battleship spaceShip) => mSpaceship = spaceShip;

        public override double GetScore()
        {
            var shieldHullScore = mSpaceship.Defense.HullPercentage * 0.95 + mSpaceship.Defense.ShieldPercentage * 0.05;
            var targets = mSpaceship.Sensors.Opponents;
            var targetScore = MathF.Min(targets.Count, 1);
            return targetScore * shieldHullScore * .1f;
        }

        public override void Execute()
        {
            var spaceship = mSpaceship.Sensors.Opponents.FirstOrDefault(defaultValue: null);
            if (spaceship == null) return;
            mSpaceship.ImpulseDrive.FollowSpaceship(spaceship);
        }

        public override void Recet() => mPatrolTarget = null;
    }
}
