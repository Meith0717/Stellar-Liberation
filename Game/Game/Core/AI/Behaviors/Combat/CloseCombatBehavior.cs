// CloseCombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;
using StellarLiberation.Game.Core.Utilitys;
using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.AI.Behaviors.Combat
{
    public class CloseCombatBehavior : CombatBehavior
    {
        private bool mReorienting;

        public CloseCombatBehavior(float attacDistance) : base(attacDistance) { }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            if (environment.AimingShip is null) return;
            mDistance = Vector2.Distance(spaceShip.Position, environment.AimingShip.Position);

            switch (mReorienting)
            {
                case true:
                    // Check for breaking reorientation
                    if (!spaceShip.SublightEngine.IsMoving) mReorienting = false;
                    break;

                case false:
                    // Check if a reorientation is needet 
                    if (mDistance < 1000)
                    {
                        mReorienting = true;
                        // Set course to reorientation Position
                        spaceShip.SublightEngine.MoveToPosition(GetReorientingPosition(environment));
                        break;
                    }

                    // Set course to attac target
                    if (mDistance < mAttackDistance) spaceShip.WeaponSystem.Fire();
                    spaceShip.SublightEngine.FollowSpaceShip(environment.AimingShip);
                    break;
            }
        }

        private Vector2 GetReorientingPosition(SensorArray environment)
        {
            var targetPos = environment.AimingShip.Position;
            return ExtendetRandom.NextVectorOnBorder(new(targetPos, 5000));
        }

        public override void Reset(SpaceShip spaceShip)
        {
            mReorienting = false;
            spaceShip.WeaponSystem.StopFire();
        }
    }
}
