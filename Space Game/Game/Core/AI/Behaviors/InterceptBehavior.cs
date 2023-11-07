// InterceptBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.Collision_Detection;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;

namespace CelestialOdyssey.Game.Core.AI.Behaviors
{
    internal class InterceptBehavior : Behavior
    {
        public override double GetScore(SensorArray environment, SpaceShip spaceShip)
        {
            var shielHhullScore = spaceShip.DefenseSystem.ShildLevel * 0.2 + spaceShip.DefenseSystem.HullLevel * 0.8;

            var distanceToAimingShip = float.IsPositiveInfinity(environment.DistanceToAimingShip)? 0 : environment.DistanceToAimingShip;

            var score = shielHhullScore * (distanceToAimingShip * 4 / environment.ShortRangeScanDistance);
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
