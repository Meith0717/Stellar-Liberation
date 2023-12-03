// FarCombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat
{
    internal class FarCombatBehavior : CombatBehavior
    {
        public FarCombatBehavior(float attacDistance) : base(attacDistance) { }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            spaceShip.SublightEngine.Standstill();
            spaceShip.WeaponSystem.StopFire();
            if (mDistance <= mAttackDistance) spaceShip.WeaponSystem.Fire();
        }

        public override void Reset(SpaceShip spaceShip) => spaceShip.WeaponSystem.StopFire();
    }
}
