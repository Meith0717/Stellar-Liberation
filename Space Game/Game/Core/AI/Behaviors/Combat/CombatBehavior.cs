// FighterAttacBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.AI.Behaviors.Combat
{
    public abstract class CombatBehavior : Behavior
    {
        protected float mAttackDistance;
        protected float mDistance;

        public CombatBehavior(float attacDistance) => mAttackDistance = attacDistance;

        public override double GetScore(SensorArray environment, SpaceShip spaceShip)
        {
            var shieldScore = spaceShip.DefenseSystem.ShildLevel * 0.4;
            var hullScore = spaceShip.DefenseSystem.HullLevel * 0.6;
            var target = environment.AimingShip;
            if (target is null)
            {
                System.Diagnostics.Debug.WriteLine($"AttacScore: {0}");
                return 0;
            }
            var targetShieldScore = target.DefenseSystem.ShildLevel * 0.1;
            var targetHullScore = target.DefenseSystem.HullLevel * 0.9;

            mDistance = Vector2.Distance(spaceShip.Position, target.Position);
            var inDistance = mDistance < mAttackDistance ? 1 : 0;

            var score = inDistance * (shieldScore + hullScore + (1 - (targetShieldScore + targetHullScore)) * 10);
            System.Diagnostics.Debug.WriteLine($"AttacScore: {score}");
            return score;
        }
    }
}
