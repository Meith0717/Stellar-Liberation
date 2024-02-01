// CombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat
{
    public abstract class CombatBehavior : Behavior
    {
        protected float mDistance;

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.4 + spaceShip.DefenseSystem.HullPercentage * 0.6;

            var target = spaceShip.WeaponSystem.AimingShip;
            if (target is null) return 0;

            var targetShielHhullScore = target.DefenseSystem.ShieldPercentage * 0.1 + target.DefenseSystem.HullPercentage * 0.9;

            mDistance = Vector2.Distance(spaceShip.Position, target.Position);
            var inAttacDistance = mDistance <= spaceShip.WeaponSystem.Range ? 1 : 0;

            var score = inAttacDistance * (shielHhullScore * 0.5 + (1 - targetShielHhullScore) * 0.5);
            return score;
        }
    }
}
