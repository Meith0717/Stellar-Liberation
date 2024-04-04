// SpaceShipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceshipManagement;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships
{
    public enum ShipID { Bomber, Cargo, Corvette, Cuiser, Destroyer, Fighter }

    public class SpaceshipFactory
    {
        public static Spaceship Get(Vector2 position, ShipID shipID, Fractions fraction)
        {
            Spaceship spaceship = shipID switch
            {
                ShipID.Bomber => new(position, fraction, GameSpriteRegistries.bomber, 1),
                ShipID.Cargo => new(position, fraction, GameSpriteRegistries.cargo, 5),
                ShipID.Corvette => new(position, fraction, GameSpriteRegistries.corvette, 1),
                ShipID.Cuiser => new(position, fraction, GameSpriteRegistries.cruiser, 1),
                ShipID.Destroyer => new(position, fraction, GameSpriteRegistries.destroyer, 5),
                ShipID.Fighter => new(position, fraction, GameSpriteRegistries.fighter, 1),
                _ => throw new System.NotImplementedException()
            };
            spaceship.ApplyConfig(1, 1, 0, 0, 1, 1);
            return spaceship;
        }
    }
}