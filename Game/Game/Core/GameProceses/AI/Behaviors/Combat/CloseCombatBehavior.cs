// CloseCombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement.ShipSystems;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat
{
    public class CloseCombatBehavior : CombatBehavior
    {
        private bool mReorienting;

        public CloseCombatBehavior(float attacDistance) : base(attacDistance) { }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            if (spaceShip.SensorArray.AimingShip is null) return;
            mDistance = Vector2.Distance(spaceShip.Position, spaceShip.SensorArray.AimingShip.Position);

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
                        spaceShip.SublightEngine.MoveToPosition(GetReorientingPosition(spaceShip.SensorArray));
                        break;
                    }

                    // Set course to attac target
                    if (mDistance < mAttackDistance) spaceShip.WeaponSystem.Fire();
                    spaceShip.SublightEngine.FollowSpaceShip(spaceShip.SensorArray.AimingShip);
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
