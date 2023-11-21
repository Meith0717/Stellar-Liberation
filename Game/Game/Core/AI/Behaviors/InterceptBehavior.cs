// InterceptBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;

namespace StellarLiberation.Game.Core.AI.Behaviors
{
    internal class InterceptBehavior : Behavior
    {
        public override double GetScore(SensorArray environment, SpaceShip spaceShip)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.2 + spaceShip.DefenseSystem.HullPercentage * 0.8;

            var distanceToAimingShip = float.IsPositiveInfinity(environment.DistanceToAimingShip)? 0 : environment.DistanceToAimingShip;

            var score = shielHhullScore * (distanceToAimingShip / environment.ShortRangeScanDistance);
            return score;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.FollowSpaceShip(environment.AimingShip);
        }


        public override void Reset(SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.Standstill();
        }
    }
}
