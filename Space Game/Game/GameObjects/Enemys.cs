// Enemys.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI.Behaviors;
using CelestialOdyssey.Game.Core.AI.Behaviors.Combat;
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
                : base(position, ContentRegistry.enemyCarrior, 4, new(20000), new(0.5f, 0.01f), new(1000, Color.IndianRed, 1, 1), new(100, 100, 0, 0))
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
                mAi = new(new()
                {
                    new PatrollBehavior(),
                    new InterceptBehavior(),
                    new FarCombatBehavior(10000),
                    new FleeBehavior()
                });
            }

            public override void HasCollide() => throw new System.NotImplementedException(); 
        }

        public class BattleShip : Enemy
        {
            public BattleShip(Vector2 position)
                : base(position, ContentRegistry.enemyBattleShip, 1, new(20000), new(0.5f, 0.01f), new(1000, Color.IndianRed, 1, 1), new(100, 100, 0, 0))
            {
                WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-150, -0), 1, TextureDepth + 1));

                mAi = new(new() {
                    new PatrollBehavior(),
                    new InterceptBehavior(),
                    new FarCombatBehavior(10000),
                    new FleeBehavior()
                });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }

        public class Bomber : Enemy
        {
            public Bomber(Vector2 position)
                : base(position, ContentRegistry.enemyBomber, 0.7f, new(20000), new(0.6f, 0.01f), new(1000, Color.IndianRed, 1, 1), new(100, 100, 0, 0))
            {
                WeaponSystem.PlaceTurret(new(new(-50, 106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-50, -106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-60, 0), 1, TextureDepth + 1));

                mAi = new(new()
                {
                    new PatrollBehavior(),
                    new InterceptBehavior(),
                    new FarCombatBehavior(10000),
                    new FleeBehavior()
                });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }

        public class Fighter : Enemy
        {
            public Fighter(Vector2 position)
                : base(position, ContentRegistry.enemyFighter, 0.2f, new(20000), new(1.5f, 0.2f), new(1000, Color.IndianRed, 1, 1), new(100, 100, 0f, 0f))
            {
                WeaponSystem.PlaceTurret(new(new(0, 0), 1, TextureDepth + 1));

                mAi = new(new() {
                    new PatrollBehavior(),
                    new CloseCombatBehavior(10000),
                    new FleeBehavior(),
                });
            }

            public override void HasCollide() => throw new System.NotImplementedException();
        }
    }
}
