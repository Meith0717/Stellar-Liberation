// EnemyFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors;
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

namespace StellarLiberation.Game.GameObjects.SpaceShips.Enemys
{
    public enum EnemyId { EnemyBattleShip, EnemyBomber, EnemyFighter, EnemyCarrior }

    public static class EnemyFactory
    {
        private class Carrior : EnemyShip
        {
            public Carrior(Vector2 position)
                : base(position, TextureRegistries.enemyCarrior, 4, new(80000), new(1f, 0.01f), new(1000, new(255, 4, 0), 1, 1, 10000), new(100, 100, 0))
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

                WeaponSystem.PlaceTurret(new(new(-900, -1000), 1, TextureDepth + 1));
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
                    new FarCombatBehavior(),
                    new SpawnFighterBehavior(20000),
                    new FleeBehavior()
                });
            }
        }

        private class BattleShip : EnemyShip
        {
            public BattleShip(Vector2 position)
                : base(position, TextureRegistries.enemyBattleShip, 1, new(50000), new(2f, 0.01f), new(1000, Color.IndianRed, 1, 1, 10000), new(100, 100, 0))
            {
                WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-150, -0), 1, TextureDepth + 1));

                mAi = new(new() {
                    new PatrollBehavior(),
                    new InterceptBehavior(),
                    new CloseCombatBehavior(),
                    new FleeBehavior()
                });
            }
        }

        private class Bomber : EnemyShip
        {
            public Bomber(Vector2 position)
                : base(position, TextureRegistries.enemyBomber, 0.7f, new(20000), new(3f, 0.01f), new(1000, Color.IndianRed, 1, 1, 10000), new(100, 100, 0))
            {
                WeaponSystem.PlaceTurret(new(new(-50, 106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-50, -106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-60, 0), 1, TextureDepth + 1));

                mAi = new(new()
                {
                    new PatrollBehavior(),
                    new InterceptBehavior(),
                    new CloseCombatBehavior(),
                    new FleeBehavior()
                });
            }
        }

        private class Fighter : EnemyShip
        {
            public Fighter(Vector2 position)
                : base(position, TextureRegistries.enemyFighter, 0.2f, new(10000), new(4f, 0.2f), new(1000, Color.IndianRed, 1, 1, 10000), new(100, 100, 0))
            {
                WeaponSystem.PlaceTurret(new(new(0, 0), 1, TextureDepth + 1));

                mAi = new(new() {
                    new InterceptBehavior(),
                    new CloseCombatBehavior(),
                })
                { Debug = true };
            }

            public override void Update(GameTime gameTime, InputState inputState, Scene scene)
            {
                base.Update(gameTime, inputState, scene);
            }
        }

        public static EnemyShip Get(EnemyId id, Vector2 position) => id switch
        {
            EnemyId.EnemyCarrior => new Carrior(position),
            EnemyId.EnemyBattleShip => new BattleShip(position),
            EnemyId.EnemyBomber => new Bomber(position),
            EnemyId.EnemyFighter => new Fighter(position),
            _ => throw new System.NotImplementedException()
        };
    }
}
