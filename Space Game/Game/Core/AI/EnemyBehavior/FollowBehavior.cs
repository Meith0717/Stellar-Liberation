using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    internal class FollowBehavior : Behavior
    {
        private readonly float mVelocity;

        public FollowBehavior(float velocity) 
        {
            mVelocity = velocity;    
        }

        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {
            spaceShip.Velocity = mVelocity;
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, spaceShip.Target.Position, 0.1f);
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        => (spaceShip.Target is null) ? 0 : 0.2;
    }
}
