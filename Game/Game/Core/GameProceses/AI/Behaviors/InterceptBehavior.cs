// InterceptBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class InterceptBehavior : Behavior
    {
        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.2 + spaceShip.DefenseSystem.HullPercentage * 0.8;

            var distanceToAimingShip = spaceShip.WeaponSystem.AimingShip is null ? 0 : Vector2.Distance(spaceShip.WeaponSystem.AimingShip.Position, spaceShip.Position);

            var score = shielHhullScore * (distanceToAimingShip / spaceShip.SensorArray.ShortRangeScanDistance);
            return score;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            spaceShip.SublightEngine.SetVelocity(1);
            spaceShip.SublightEngine.FollowSpaceShip(spaceShip.WeaponSystem.AimingShip);
        }


        public override void Reset(SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.Standstill();
        }
    }
}
