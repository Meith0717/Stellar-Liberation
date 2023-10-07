using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.Core.GameObjects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class BattleShipAttacBehavior : Behavior
    {
        private readonly float mAttacDistance;

        public BattleShipAttacBehavior(int attacDistance) 
        { 
            mAttacDistance = attacDistance;
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        {
            if (spaceShip.Target is null) return 0;
            var distanceToTarget = Vector2.Distance(spaceShip.Target.Position, spaceShip.Position);
            if (distanceToTarget > mAttacDistance) return 0;
            return 0.5;
        }

        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {
            spaceShip.WeaponSystem.Fire(spaceShip, spaceShip.Target.Position);
        }

    }
}
