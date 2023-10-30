// Behavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;

namespace CelestialOdyssey.Game.Core.AI
{
    public abstract class Behavior
    {
        public abstract double GetPriority(SensorArray environment, SpaceShip spaceShip);
        public abstract void Execute(SensorArray environment, SpaceShip spaceShip);
        public abstract void Reset();
    }
}
