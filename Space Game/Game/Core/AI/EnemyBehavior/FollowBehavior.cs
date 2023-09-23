using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FollowBehavior : Behavior
    {
        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {
            spaceShip.Velocity = 50;
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, spaceShip.Target.Position, 0.1f);
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        => (spaceShip.Target is null) ? 0 : 0.2;
    }
}
