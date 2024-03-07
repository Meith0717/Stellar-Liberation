﻿// SpaceshipController.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers.GameLayers;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components
{
    public class SpaceshipController
    {
        private float mVelocityProcentage;

        public void Controll(GameTime gameTime, Spaceship spaceShip, InputState inputState, GameLayer gameLayer)
        {
            if (spaceShip == null) return;
            spaceShip.PhaserCannaons.StopFire();
            inputState.DoAction(ActionType.Accelerate, () => mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage + .002f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0, 1));
            inputState.DoAction(ActionType.Break, () => mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage - .002f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0, 1));
            inputState.DoAction(ActionType.Inventar, () => gameLayer.LayerManager.AddLayer(new InventoryLayer(spaceShip.Inventory, gameLayer.GameState.Wallet)));
            inputState.DoAction(ActionType.RightClickHold, () => spaceShip.PhaserCannaons.Fire());

            spaceShip.SublightDrive.SetVelocity(mVelocityProcentage);
            spaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(gameLayer.WorldMousePosition - spaceShip.Position));
            spaceShip.SensorSystem.TryGetAimingShip(spaceShip.Position, out var target);
            gameLayer.Camera2D.Position = spaceShip.Position;
        }
    }
}
