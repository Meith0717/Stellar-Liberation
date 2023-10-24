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
                : base(position, ContentRegistry.enemyBattleShip, 1, new(10000, 10000), new(0.5f, 0.01f), new(1, 1), new(Color.BlueViolet, 10, 10, 100, 1000), new(1000, 1000, 1, 1))
            {
                WeaponSystem.SetWeapon(new(110, 35));
                WeaponSystem.SetWeapon(new(110, -35));
                WeaponSystem.SetWeapon(new(-130, 100));
                WeaponSystem.SetWeapon(new(-130, -100));
                WeaponSystem.SetWeapon(new(-150, 0));

                mAi = new(new() {
               new SearchBehavior(),
               new FleeBehavior(50000),
               new AttacBehavior()});
            }

            public override void HasCollide()
            {
                throw new System.NotImplementedException();
            }
        }

        public class Bomber : Enemy
        {
            public Bomber(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 0.7f, new(10000, 10000), new(0.6f, 0.01f), new(1, 1), new(Color.BlueViolet, 10, 10, 100, 1000), new(100, 100, 1, 1))
            {
                WeaponSystem.SetWeapon(new(-50, 106));
                WeaponSystem.SetWeapon(new(-50, -106));
                WeaponSystem.SetWeapon(new(-60, 0));

                mAi = new(new() {
               new SearchBehavior(),
               new FleeBehavior(50000),
               new AttacBehavior()});
            }

            public override void HasCollide()
            {
                throw new System.NotImplementedException();
            }
        }

        public class Fighter : Enemy
        {
            public Fighter(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 0.3f, new(10000, 10000), new(0.7f, 0.1f), new(1, 1), new(Color.BlueViolet, 1, 1, 100, 100), new(1000, 1000, 1, 1))
            {
                WeaponSystem.SetWeapon(new(0, 0));

                mAi = new(new() {
               new SearchBehavior(),
               new FleeBehavior(100000),
               new AttacBehavior()});
            }

            public override void HasCollide()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
