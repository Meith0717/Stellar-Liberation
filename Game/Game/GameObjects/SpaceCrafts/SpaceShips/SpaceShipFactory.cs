// SpaceshipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceshipManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships
{
    public enum ShipID { Bomber, Cargo, Corvette, Cuiser, Destroyer, Fighter }

    public class SpaceshipFactory
    {
        public static void Spawn(PlanetSystem planetSystem, Vector2 position, ShipID shipID, Fractions fraction, out Spaceship spaceShip)
        {
            spaceShip = Get(position, shipID, fraction);
            spaceShip.PlanetSystem = planetSystem;
            planetSystem.GameObjects.Add(spaceShip);
        }

        public static Spaceship Get(Vector2 position, ShipID shipID, Fractions fraction)
        {
            SpaceshipConfig config = shipID switch
            {
                ShipID.Bomber => new(
                    textureID: GameSpriteRegistries.bomber,
                    textureScale: 1,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 300,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    }
                ),
                ShipID.Cargo => new(
                    textureID: GameSpriteRegistries.cargo,
                    textureScale: 1,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    }
                ),
                ShipID.Corvette => new(
                    textureID: GameSpriteRegistries.corvette,
                    textureScale: 1,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    }
                ),
                ShipID.Cuiser => new(
                    textureID: GameSpriteRegistries.cruiser,
                    textureScale: 1,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    }
                ),
                ShipID.Destroyer => new(
                    textureID: GameSpriteRegistries.destroyer,
                    textureScale: 1,
                    velocity: 5,
                    turretCoolDown: 50,
                    shieldForce: 10000,
                    hullForce: 100000,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    }
                ),
                ShipID.Fighter => new(
                    textureID: GameSpriteRegistries.fighter,
                    textureScale: 1,
                    velocity: 5,
                    turretCoolDown: 50,
                    shieldForce: 100,
                    hullForce: 100,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    }
                ),
                _ => throw new System.NotImplementedException()
            };
            return new Spaceship(position, fraction, config);
        }
    }
}