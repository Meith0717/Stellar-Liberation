// Enemys.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects
{
    public class Enemys
    {
        public class Carrior : Enemy
        {
            public Carrior(Vector2 position)
                : base(position, ContentRegistry.enemyCarrior, 4, new(10000), new(0.5f, 0.01f), new(300, Color.IndianRed, 10, 10), new(5000, 5000, 1, 1))
            {
                WeaponSystem.PlaceTurret(new(new(1100, 600), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(975, 600), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(850, 600), 1, TextureDepth + 1));

                WeaponSystem.PlaceTurret(new(new(1100, -600), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(975, -600), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(850, -600), 1, TextureDepth + 1));

                WeaponSystem.PlaceTurret(new(new(-900, 1000), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-775, 950), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-650, 900), 1, TextureDepth + 1));
                
                WeaponSystem.PlaceTurret(new(new(-900, -1000), 1, TextureDepth + 1  ));
                WeaponSystem.PlaceTurret(new(new(-775, -950), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-650, -900), 1, TextureDepth + 1));

                WeaponSystem.PlaceTurret(new(new(0, 50), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(100, 50), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(200, 50), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(300, 50), 1, TextureDepth + 1));

                WeaponSystem.PlaceTurret(new(new(0, 50), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-100, 50), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-200, 50), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-300, 50), 1, TextureDepth + 1));
                mAi = new(new() { });
            }

            public override void HasCollide() => throw new System.NotImplementedException(); 
        }

        public class BattleShip : Enemy
        {
            public BattleShip(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 1, new(10000), new(0.5f, 0.01f), new(300, Color.IndianRed, 10, 10), new(1000, 1000, 1, 1))
            {
                WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-150, -0), 1, TextureDepth + 1));

                mAi = new(new() { });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }

        public class Bomber : Enemy
        {
            public Bomber(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 0.7f, new(10000), new(0.6f, 0.01f), new(300, Color.IndianRed, 10, 10), new(100, 100, 1, 1))
            {
                WeaponSystem.PlaceTurret(new(new(-50, 106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-50, -106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-60, 0), 1, TextureDepth + 1));

                mAi = new(new() { });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }

        public class Fighter : Enemy
        {
            public Fighter(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 0.3f, new(10000), new(0.7f, 0.1f), new(300, Color.IndianRed, 10, 10), new(1000, 1000, 1, 1))
            {
                WeaponSystem.PlaceTurret(new(new(0, 0), 1, TextureDepth + 1));

                mAi = new(new() {
                    new SearchBehavior(),
                    new AttacBehavior()
                });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }
    }
}
