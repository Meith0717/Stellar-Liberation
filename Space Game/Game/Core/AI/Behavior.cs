
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.AI
{
    public abstract class Behavior
    {
        public abstract double GetPriority(SensorArray environment, SpaceShip spaceShip);
        public abstract void Execute(SensorArray environment, SpaceShip spaceShip);
    }
}
