﻿using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public class EnemyCorvette : Enemy
    {
        public EnemyCorvette(Vector2 position)
            : base(position, ContentRegistry.enemyCorvette, 75)
        {
            SensorArray = new(2000000, Configs.SensorArrayCoolDown);

            WeaponSystem = new(Color.Red, 1, 1, 500);
            WeaponSystem.SetWeapon(new(0, 0));

            SublightEngine = new(50);

            mAi = new(new()
            {
               new PartolBehavior(),
               new FollowBehavior(),
               new FighterAttacBehavior(250000)
            }) ;
        }
    }
}