// AlliedFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors;
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors.Combat;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Allies;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys
{
    public enum AlliedId { AlliedBattleShip, AlliedBomber, AlliedFighter, AlliedCarrior }

    public static class AlliedFactory
    {
        private class Carrior : AlliedShip
        {
            internal Carrior(Vector2 position)
                : base(position, GameSpriteRegistries.destroyer, 4, new(80000), new(1f, 0.01f), new(1000, new(120, 200, 240), 1, 1, 10000), new(100, 100, 0), new(5000))
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

                mUtilityAi.AddBehavior(new PatrollBehavior());
                mUtilityAi.AddBehavior(new InterceptBehavior());
                mUtilityAi.AddBehavior(new FarCombatBehavior());
                mUtilityAi.AddBehavior(new SpawnFighterBehavior(20000));
                mUtilityAi.AddBehavior(new FleeBehavior());
            }
        }

        internal class BattleShip : AlliedShip
        {
            public BattleShip(Vector2 position)
                : base(position, GameSpriteRegistries.cruiser, 1, new(50000), new(2f, 0.01f), new(1000, new(120, 200, 240), 1, 1, 10000), new(100, 100, 0), new(5000))
            {
                WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-150, -0), 1, TextureDepth + 1));

                mUtilityAi.AddBehavior(new PatrollBehavior());
                mUtilityAi.AddBehavior(new InterceptBehavior());
                mUtilityAi.AddBehavior(new CloseCombatBehavior());
                mUtilityAi.AddBehavior(new FleeBehavior());
            }
        }

        internal class Bomber : AlliedShip
        {
            public Bomber(Vector2 position)
                : base(position, GameSpriteRegistries.bomber, 0.7f, new(20000), new(3f, 0.01f), new(1000, new(120, 200, 240), 1, 1, 10000), new(100, 100, 0), new(5000))
            {
                WeaponSystem.PlaceTurret(new(new(-50, 106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-50, -106), 1, TextureDepth + 1));
                WeaponSystem.PlaceTurret(new(new(-60, 0), 1, TextureDepth + 1));

                mUtilityAi.AddBehavior(new PatrollBehavior());
                mUtilityAi.AddBehavior(new InterceptBehavior());
                mUtilityAi.AddBehavior(new CloseCombatBehavior());
                mUtilityAi.AddBehavior(new FleeBehavior());
            }
        }

        internal class Fighter : AlliedShip
        {
            public Fighter(Vector2 position)
                : base(position, GameSpriteRegistries.fighter, 0.5f, new(10000), new(4f, 0.2f), new(1000, new(120, 200, 240), 1, 1, 10000), new(20, 20, 0), new(5000))
            {
                WeaponSystem.PlaceTurret(new(new(0, 0), 1, TextureDepth + 1));

                mUtilityAi.AddBehavior(new InterceptBehavior());
                mUtilityAi.AddBehavior(new CloseCombatBehavior());
            }
        }

        internal static AlliedShip Get(AlliedId id, Vector2 position) => id switch
        {
            AlliedId.AlliedCarrior => new Carrior(position),
            AlliedId.AlliedBattleShip => new BattleShip(position),
            AlliedId.AlliedBomber => new Bomber(position),
            AlliedId.AlliedFighter => new Fighter(position),
            _ => throw new System.NotImplementedException()
        };
    }
}
