// FarCombatBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat
{
    internal class FarCombatBehavior : CombatBehavior
    {
        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            spaceShip.SublightDrive.SetVelocity(.1f);
            spaceShip.WeaponSystem.StopFire();
            if (mDistance <= spaceShip.WeaponSystem.Range) spaceShip.WeaponSystem.Fire();
        }

        public override void Reset(SpaceShip spaceShip) => spaceShip.WeaponSystem.StopFire();
    }
}
