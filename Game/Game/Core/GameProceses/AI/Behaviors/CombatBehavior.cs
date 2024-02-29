// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CombatBehavior : Behavior
    {
        protected float mDistance;

        private readonly SpaceShip mSpaceShip;

        public CombatBehavior(SpaceShip spaceShip) => mSpaceShip = spaceShip;

        public override double GetScore()
        {
            mSpaceShip.WeaponSystem.StopFire();
            if (!mSpaceShip.SensorSystem.TryGetAimingShip(mSpaceShip.Position, out var target)) 
            {
                mSpaceShip.WeaponSystem.ClearAiming();
                return 0;
            }
            mSpaceShip.WeaponSystem.AimShip(target);

            var shielHhullScore = mSpaceShip.DefenseSystem.ShieldPercentage * 0.4 + mSpaceShip.DefenseSystem.HullPercentage * 0.6;
            var targetShielHhullScore = target.DefenseSystem.ShieldPercentage * 0.1 + target.DefenseSystem.HullPercentage * 0.9;

            var score = shielHhullScore * 0.5 + (1 - targetShielHhullScore) * 0.5;
            return score;
        }


        public override void Execute()
        {
            mSpaceShip.SublightDrive.SetVelocity(.1f);
            mSpaceShip.SublightDrive.FollowSpaceShip(mSpaceShip.WeaponSystem.AimingShip);
            mSpaceShip.WeaponSystem.Fire();
        }
    }
}
