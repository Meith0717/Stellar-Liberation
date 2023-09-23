
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.AI
{
    public abstract class Behavior
    {
        public abstract double GetPriority(List<GameObject> environment, SpaceShip spaceShip);
        public abstract void Execute(List<GameObject> environment, SpaceShip spaceShip);
    }
}
