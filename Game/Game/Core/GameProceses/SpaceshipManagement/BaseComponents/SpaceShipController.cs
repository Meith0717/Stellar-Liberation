// SpaceShipController.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components
{
    public class SpaceshipController
    {
        private float mVelocityProcentage;

        public void Controll(GameTime gameTime, Spaceship spaceShip, InputState inputState, GameLayer gameLayer)
        {
            if (spaceShip == null) return;
            if (inputState.HasAction(ActionType.Accelerate))
            {
                mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage + .002f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0, 1);
            }
            else
            {
                mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage - .002f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0, 1);
            }

            spaceShip.SublightDrive.SetVelocity(mVelocityProcentage);
            spaceShip.SublightDrive.MoveToTarget(spaceShip.Position);
            spaceShip.SensorSystem.TryGetAimingShip(spaceShip.Position, out var target);
            gameLayer.Camera2D.Position = spaceShip.Position;
        }
    }
}
