﻿// InterceptBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;

namespace StellarLiberation.Game.Core.AI.Behaviors
{
    internal class InterceptBehavior : Behavior
    {
        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.2 + spaceShip.DefenseSystem.HullPercentage * 0.8;

            var distanceToAimingShip = float.IsPositiveInfinity(spaceShip.SensorArray.DistanceToAimingShip)? 0 : spaceShip.SensorArray.DistanceToAimingShip;

            var score = shielHhullScore * (distanceToAimingShip / spaceShip.SensorArray.ShortRangeScanDistance);
            return score;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            spaceShip.SublightEngine.FollowSpaceShip(spaceShip.SensorArray.AimingShip);
        }


        public override void Reset(SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.Standstill();
        }
    }
}
