using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
{
    public class SublightEngine
    {
        private float mMaxVelocity;

        public SublightEngine(float maxVelocity)
        {
            mMaxVelocity = maxVelocity;
        }

        public void FollowMouse(InputState inputState, SpaceShip spaceShip, Vector2 worldMousePosition)
        {
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, worldMousePosition, .1f);
            if (inputState.HasMouseAction(MouseActionType.RightClickHold))
            {
                if (spaceShip.Velocity <= 0) spaceShip.Velocity = 1;
                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, mMaxVelocity / 50);
                if (spaceShip.Velocity > mMaxVelocity) spaceShip.Velocity = mMaxVelocity;
                return;
            }
            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, -mMaxVelocity / 50);
            if (spaceShip.Velocity < 0) spaceShip.Velocity = 0;
        }
    }
}
