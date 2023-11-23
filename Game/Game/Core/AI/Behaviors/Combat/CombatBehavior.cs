﻿// CombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;

namespace StellarLiberation.Game.Core.AI.Behaviors.Combat
{
    public abstract class CombatBehavior : Behavior
    {
        protected float mAttackDistance;
        protected float mDistance;

        public CombatBehavior(float attacDistance) => mAttackDistance = attacDistance;

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.4 + spaceShip.DefenseSystem.HullPercentage * 0.6;

            var target = spaceShip.SensorArray.AimingShip;
            if (target is null)
            {
                return 0;
            }
            var targetShielHhullScore = target.DefenseSystem.ShieldPercentage * 0.1 + target.DefenseSystem.HullPercentage * 0.9;

            mDistance = Vector2.Distance(spaceShip.Position, target.Position);
            var inAttacDistance = mDistance < mAttackDistance ? 1 : 0;

            var score = inAttacDistance * (shielHhullScore * 0.5 + (1 - targetShielHhullScore) * 0.5);
            return score;
        }
    }
}
