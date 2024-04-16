// SpacecraftFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    public enum BattleshipID { BomberMKI, InterceptorMKI, FighterMKI, BomberMKII, InterceptorMKII, FighterMKII }
    public enum FlagshipID { }

    public class SpacecraftFactory
    {
        public static Battleship GetBattleship(Vector2 position, BattleshipID shipID, Fractions fraction)
        {
            Battleship spaceship = shipID switch
            {
                BattleshipID.BomberMKI => new(position, fraction, GameSpriteRegistries.bomber, 1),
                BattleshipID.BomberMKII => new(position, fraction, GameSpriteRegistries.bomber, 1),
                BattleshipID.InterceptorMKI => new(position, fraction, GameSpriteRegistries.destroyer, 5),
                BattleshipID.InterceptorMKII => new(position, fraction, GameSpriteRegistries.destroyer, 5),
                BattleshipID.FighterMKI => new(position, fraction, GameSpriteRegistries.fighter, 1),
                BattleshipID.FighterMKII => new(position, fraction, GameSpriteRegistries.fighter, 1),
                _ => throw new System.NotImplementedException()
            };
            spaceship.Populate(200, 200, 0, 0, new(), 1);
            return spaceship;
        }

        public static Flagship GetFlagship(Vector2 position, Fractions fraction)
        {
            Flagship spaceship = new(position, fraction);
            var weapons = new List<Weapon>()
            {
                new(new(-290, 610), GameSpriteRegistries.turette, GameSpriteRegistries.projectile, Color.MonoGameOrange, false, 10, 10, 50000, 200),
                new(new(-290, -610), GameSpriteRegistries.turette, GameSpriteRegistries.projectile, Color.MonoGameOrange, false, 10, 10, 50000, 200),
            };
            spaceship.Populate(100, 100, 1, 0, weapons, 1, 1, 10);
            return spaceship;
        }
    }
}