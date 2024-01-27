// SpaceShipFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    public enum ShipID { Bomber, Cargo, Corvette, Cuiser, Destroyer, Fighter }

    public class SpaceShipFactory
    {
        public static SpaceShip Get(Vector2 position, ShipID shipID, Fractions fraction)
        {
            SpaceShipConfig config = shipID switch
            {
                ShipID.Bomber => new(
                    textureID: GameSpriteRegistries.bomber,
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
                    {
                        new PatrollBehavior(),
                    }
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
                    {
                        new PatrollBehavior(),
                    }
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
                    {
                        new PatrollBehavior(),
                    }
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
                    {
                        new PatrollBehavior(),
                    }
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
                    {
                        new PatrollBehavior(),
                    }
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
                    {
                        new PatrollBehavior(),
                    }
                ),
                _ => throw new System.NotImplementedException()
            };
            return new(position, config);
        }
    }
}