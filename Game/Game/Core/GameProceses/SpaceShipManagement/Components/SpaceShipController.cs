﻿// SpaceShipController.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using StellarLiberation.Game.Layers.GameLayers;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components
{
    public class SpaceShipController
    {
        private float mVelocityProcentage;

        public void Controll(SpaceShip spaceShip, InputState inputState, GameLayer gameLayer)
        {
            if (spaceShip == null) return;
            inputState.DoAction(ActionType.Accelerate, () => mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage + .01f, 0, 1));
            inputState.DoAction(ActionType.Break, () => mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage - .01f, 0, 1));
            inputState.DoAction(ActionType.Inventar, () => gameLayer.LayerManager.AddLayer(new InventoryLayer(spaceShip.Inventory, gameLayer.GameState.Wallet)));

            spaceShip.SublightDrive.SetVelocity(mVelocityProcentage);
            spaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(gameLayer.WorldMousePosition - spaceShip.Position));
            spaceShip.SensorSystem.TryGetAimingShip(spaceShip.Position, out var target);
            spaceShip.PhaserCannaons.ControlByInput(inputState);
            gameLayer.Camera2D.Position = spaceShip.Position;
        }
    }
}
