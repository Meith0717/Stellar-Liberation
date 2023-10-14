// EnemyCorvette.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class EnemyCorvette : Enemy
    {
        public EnemyCorvette(Vector2 position)
            : base(position, ContentRegistry.enemyBomber, 5f)
        {
            SensorArray = new(10000, 1000);

            WeaponSystem = new(Color.Red, 1, 1, 500);
            WeaponSystem.SetWeapon(new(0, 0));

            SublightEngine = new(5);

            mAi = new(new()
            {
               new PartolBehavior(),
               new FollowBehavior(),
               new FighterAttacBehavior(8000)
            });
        }

        public override void HasCollide()
        {
            throw new System.NotImplementedException();
        }
    }
}
