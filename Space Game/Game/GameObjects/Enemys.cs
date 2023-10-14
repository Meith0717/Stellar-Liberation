// Enemys.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects
{
    public class Enemys
    {
        public class BattleShip : Enemy
        {
            public BattleShip(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 1)
            {
                SensorArray = new(10000, 1000);

                WeaponSystem = new(Color.Red, 1, 1, 500);
                WeaponSystem.SetWeapon(new(110, 35));
                WeaponSystem.SetWeapon(new(110, -35));
                WeaponSystem.SetWeapon(new(-130, 100));
                WeaponSystem.SetWeapon(new(-130, -100));
                WeaponSystem.SetWeapon(new(-150, 0));

                SublightEngine = new(5);

                mAi = new(new() {
               new PartolBehavior(),
               new FollowBehavior(),
               new FleeBehavior(50000),
               new FighterAttacBehavior(8000)});
            }

            public override void HasCollide()
            {
                throw new System.NotImplementedException();
            }
        }

        public class Bomber : Enemy
        {
            public Bomber(Vector2 position)
                : base(position, ContentRegistry.enemyBomber, 1f)
            {
                SensorArray = new(10000, 1000);

                WeaponSystem = new(Color.Red, 5, 5, 1000);
                WeaponSystem.SetWeapon(new(-50, 106));
                WeaponSystem.SetWeapon(new(-50, -106));
                WeaponSystem.SetWeapon(new(-60, 0));

                SublightEngine = new(1.5f);

                mAi = new(new() {
               new PartolBehavior(),
               new FollowBehavior(),
               new FleeBehavior(50000),
               new FighterAttacBehavior(8000)});
            }

            public override void HasCollide()
            {
                throw new System.NotImplementedException();
            }
        }

        public class Fighter : Enemy
        {
            public Fighter(Vector2 position)
                : base(position, ContentRegistry.enemyFighter, 0.75f)
            {
                SensorArray = new(10000, 1000);

                WeaponSystem = new(Color.Red, 1, 1, 50);
                WeaponSystem.SetWeapon(new(0, 0));

                SublightEngine = new(2);

                mAi = new(new() {
               new PartolBehavior(),
               new FollowBehavior(),
               new FleeBehavior(50000),
               new FighterAttacBehavior(8000)});
            }

            public override void HasCollide()
            {
                throw new System.NotImplementedException();
            }
        }
    }



}
