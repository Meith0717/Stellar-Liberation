// Behavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;

namespace StellarLiberation.Game.Core.AI
{
    public abstract class Behavior
    {
        public abstract double GetScore(SensorArray environment, SpaceShip spaceShip);
        public abstract void Execute(SensorArray environment, SpaceShip spaceShip);
        public abstract void Reset(SpaceShip spaceShip);
    }
}
