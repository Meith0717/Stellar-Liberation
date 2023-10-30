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
        public class Carrior : Enemy
        {
            public Carrior(Vector2 position)
                : base(position, ContentRegistry.enemyCarrior, 4, new(10000), new(0.5f, 0.01f), new(Color.BlueViolet, 10, 10, 300, 5000), new(5000, 5000, 1, 1))
            {
                WeaponSystem.SetWeapon(new(1100, 600));
                WeaponSystem.SetWeapon(new(975, 600));
                WeaponSystem.SetWeapon(new(850, 600));

                WeaponSystem.SetWeapon(new(1100, -600));
                WeaponSystem.SetWeapon(new(975, -600));
                WeaponSystem.SetWeapon(new(850, -600));

                WeaponSystem.SetWeapon(new(-900, 1000));
                WeaponSystem.SetWeapon(new(-775, 950));
                WeaponSystem.SetWeapon(new(-650, 900));

                WeaponSystem.SetWeapon(new(-900, -1000));
                WeaponSystem.SetWeapon(new(-775, -950));
                WeaponSystem.SetWeapon(new(-650, -900));

                WeaponSystem.SetWeapon(new(0, 50));
                WeaponSystem.SetWeapon(new(-100, 50));
                WeaponSystem.SetWeapon(new(-200, 50));
                WeaponSystem.SetWeapon(new(-300, 50));

                WeaponSystem.SetWeapon(new(0, -50));
                WeaponSystem.SetWeapon(new(-100, -50));
                WeaponSystem.SetWeapon(new(-200, -50));
                WeaponSystem.SetWeapon(new(-300, -50));

                mAi = new(new() { });
            }

            public override void HasCollide() => throw new System.NotImplementedException(); 
        }

        public class BattleShip : Enemy
        {
            public BattleShip(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 1, new(10000), new(0.5f, 0.01f), new(Color.BlueViolet, 10, 10, 100, 1000), new(1000, 1000, 1, 1))
            {
                WeaponSystem.SetWeapon(new(110, 35));
                WeaponSystem.SetWeapon(new(110, -35));
                WeaponSystem.SetWeapon(new(-130, 100));
                WeaponSystem.SetWeapon(new(-130, -100));
                WeaponSystem.SetWeapon(new(-150, 0));

                mAi = new(new() { });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }

        public class Bomber : Enemy
        {
            public Bomber(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 0.7f, new(10000), new(0.6f, 0.01f), new(Color.BlueViolet, 10, 10, 100, 1000), new(100, 100, 1, 1))
            {
                WeaponSystem.SetWeapon(new(-50, 106));
                WeaponSystem.SetWeapon(new(-50, -106));
                WeaponSystem.SetWeapon(new(-60, 0));

                mAi = new(new() { });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }

        public class Fighter : Enemy
        {
            public Fighter(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 0.3f, new(10000), new(0.7f, 0.1f), new(Color.BlueViolet, 1, 1, 100, 100), new(1000, 1000, 1, 1))
            {
                WeaponSystem.SetWeapon(new(0, 0));

                mAi = new(new() {
                    new SearchBehavior(),
                    new AttacBehavior()
                });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }
    }
}
