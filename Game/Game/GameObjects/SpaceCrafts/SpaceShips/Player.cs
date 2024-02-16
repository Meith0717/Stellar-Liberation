// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {
        private readonly SpaceShipController mSpaceShipController;

        public Player() : base(Vector2.Zero, new(
                    textureID: GameSpriteRegistries.destroyer,
                    textureScale: 1,
                    sensorRange: 15000,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 1000,
                    fraction: Fractions.Allied,
                    turretPositions: new()
                    {
                        new(110, 35),
                        new(110, -35),
                        new(-130, 100),
                        new(-130, -100),
                        new(-150, 0)
                    },
                    aiBehaviors: new()
                ))
        {
            mSpaceShipController = new();
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            scene.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            mSpaceShipController.Controll(this, inputState, scene.WorldMousePosition);
            WeaponSystem.AimShip(SensorSystem.GetAimingShip(Position));
            WeaponSystem.ControlByInput(inputState, scene.WorldMousePosition);
            scene.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            base.Update(gameTime, inputState, scene);
        }
    }
}
