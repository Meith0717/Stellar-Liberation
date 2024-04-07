// SpaceShipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipComponents.Weapons;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    public enum BattleshipID { Bomber, Interceptor, Fighter }

    public class SpacecraftFactory
    {
        public static Battleship GetBattleship(Vector2 position, BattleshipID shipID, Fractions fraction)
        {
            Battleship spaceship = shipID switch
            {
                BattleshipID.Bomber => new(position, fraction, GameSpriteRegistries.bomber, 1),
                BattleshipID.Interceptor => new(position, fraction, GameSpriteRegistries.destroyer, 5),
                BattleshipID.Fighter => new(position, fraction, GameSpriteRegistries.fighter, 1),
                _ => throw new System.NotImplementedException()
            };
            spaceship.Populate(1, 1, 0, 0, new(), 1, 1);
            return spaceship;
        }

        public static Flagship GetFlagship(Vector2 position, Fractions fraction)
        {
            Flagship spaceship = new(position, fraction);
            var weapons = new List<Weapon>()
            {
                new(Vector2.Zero, GameSpriteRegistries.turette, GameSpriteRegistries.projectile, Color.MonoGameOrange, false, 10, 10, 200),
            };
            spaceship.Populate(1, 1, 0, 0, weapons, 1, 1);
            return spaceship;
        }
    }
}