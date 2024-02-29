// SpaceShipController.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components
{
    public class SpaceShipController
    {
        private float mVelocityProcentage;

        public void Controll(SpaceShip spaceShip, InputState inputState, Vector2 worldMousePosition)
        {
            inputState.DoAction(ActionType.Accelerate, () => mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage + .01f, 0, 1));
            inputState.DoAction(ActionType.Break, () => mVelocityProcentage = MathHelper.Clamp(mVelocityProcentage - .01f, 0, 1));

            spaceShip.SublightDrive.SetVelocity(mVelocityProcentage);
            spaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(worldMousePosition - spaceShip.Position));
        }
    }
}
