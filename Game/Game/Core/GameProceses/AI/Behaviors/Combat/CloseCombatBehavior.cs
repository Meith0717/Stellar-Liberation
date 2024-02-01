// CloseCombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat
{
    public class CloseCombatBehavior : CombatBehavior
    {
        private bool mReorienting;

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            if (spaceShip.WeaponSystem.AimingShip is null) return;
            mDistance = Vector2.Distance(spaceShip.Position, spaceShip.WeaponSystem.AimingShip.Position);

            switch (mReorienting)
            {
                case true:
                    // Check for breaking reorientation
                    if (!spaceShip.SublightDrive.IsMoving) mReorienting = false;
                    spaceShip.SublightDrive.SetVelocity(1f);
                    break;

                case false:
                    // Check if a reorientation is needet 
                    if (mDistance < 1000)
                    {
                        mReorienting = true;
                        // Set course to reorientation Position
                        spaceShip.SublightDrive.MoveInDirection(GetReorientingDirection(spaceShip));
                        break;
                    }

                    spaceShip.SublightDrive.Standstill();
                    // Set course to attac target
                    if (mDistance < spaceShip.WeaponSystem.Range) spaceShip.WeaponSystem.Fire();
                    spaceShip.SublightDrive.FollowSpaceShip(spaceShip.WeaponSystem.AimingShip);
                    break;
            }
        }

        private Vector2 GetReorientingDirection(SpaceShip spaceShip)
        {
            var targetPos = spaceShip.WeaponSystem.AimingShip.Position;
            return Vector2.Normalize(targetPos - spaceShip.Position);
        }

        public override void Reset(SpaceShip spaceShip)
        {
            mReorienting = false;
            spaceShip.WeaponSystem.StopFire();
        }
    }
}
