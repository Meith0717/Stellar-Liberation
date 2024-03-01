// SpaceShipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    public enum ShipID { Bomber, Cargo, Corvette, Cuiser, Destroyer, Fighter }

    public class SpaceShipFactory
    {
        public static void Spawn(PlanetSystem planetSystem, Vector2 position, ShipID shipID, Fractions fraction, out SpaceShip spaceShip)
        {
            spaceShip = Get(position, shipID, fraction);
            spaceShip.PlanetSystem = planetSystem;
            planetSystem.GameObjects.Add(spaceShip);
        }

        public static SpaceShip Get(Vector2 position, ShipID shipID, Fractions fraction)
        {
            SpaceShipConfig config = shipID switch
            {
                ShipID.Bomber => new(
                    textureID: GameSpriteRegistries.bomber,
                    textureScale: 1,
                    sensorRange: 10000,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 300,
                    fraction: fraction,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    },
                    aiBehaviors: new()
                ),
                ShipID.Cargo => new(
                    textureID: GameSpriteRegistries.cargo,
                    textureScale: 1,
                    sensorRange: 5000,
                    velocity: 1,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    fraction: fraction,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    },
                    aiBehaviors: new()
                ),
                ShipID.Corvette => new(
                    textureID: GameSpriteRegistries.corvette,
                    textureScale: 1,
                    sensorRange: 5000,
                    velocity: 1,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    fraction: fraction,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    },
                    aiBehaviors: new()
                ),
                ShipID.Cuiser => new(
                    textureID: GameSpriteRegistries.cruiser,
                    textureScale: 1,
                    sensorRange: 5000,
                    velocity: 1,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    fraction: fraction,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    },
                    aiBehaviors: new()
                ),
                ShipID.Destroyer => new(
                    textureID: GameSpriteRegistries.destroyer,
                    textureScale: 1,
                    sensorRange: 5000,
                    velocity: 1,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    fraction: fraction,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    },
                    aiBehaviors: new()
                ),
                ShipID.Fighter => new(
                    textureID: GameSpriteRegistries.fighter,
                    textureScale: 1,
                    sensorRange: 5000,
                    velocity: 1,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    fraction: fraction,
                    turretPositions: new()
                    {
                        Vector2.Zero
                    },
                    aiBehaviors: new()
                ),
                _ => throw new System.NotImplementedException()
            };
            return new SpaceShip(position, config);
        }
    }
}