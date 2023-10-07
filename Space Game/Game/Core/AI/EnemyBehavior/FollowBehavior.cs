using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FollowBehavior : Behavior
    {
        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        => (spaceShip.Target is null) ? 0 : 0.2;

        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {
            spaceShip.SublightEngine.SetTarget(spaceShip.Target.Position);
        }
    }
}
