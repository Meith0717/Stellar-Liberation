// SpaceShipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts
{
    public enum BattleshipID { Bomber, Interceptor, Fighter }

    public class SpacecraftFactory
    {

        public static Battleship Get(Vector2 position, BattleshipID shipID, Fractions fraction)
        {
            Battleship spaceship = shipID switch
            {
                BattleshipID.Bomber => new(position, fraction, GameSpriteRegistries.bomber, 1),
                BattleshipID.Interceptor => new(position, fraction, GameSpriteRegistries.destroyer, 5),
                BattleshipID.Fighter => new(position, fraction, GameSpriteRegistries.fighter, 1),
                _ => throw new System.NotImplementedException()
            };
            spaceship.ApplyConfig(1, 1, 0, 0, 1, 1);
            return spaceship;
        }

        public static Flagship GetFlagship(Vector2 position, Fractions fraction)
        {
            Flagship spaceship = new(position, fraction);
            spaceship.ApplyConfig(1, 1, 0, 0, 1, 1);
            return spaceship;
        }
    }
}