using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
{
    internal class SublightEngine
    {
        private float mMaxVelocity;

        public SublightEngine(float maxVelocity)
        {
            mMaxVelocity = maxVelocity;
        }

        public void Update(InputState inputState, Player player, Vector2 worldMousePosition)
        {

            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                player.Rotation += MovementController.GetRotationUpdate(player.Rotation, Geometry.AngleBetweenVectors(player.Position, worldMousePosition), .1f);
                if (player.Velocity <= 0) player.Velocity = 1;
                player.Velocity = MovementController.GetVelocity(player.Velocity, mMaxVelocity / 50);
                if (player.Velocity > mMaxVelocity) player.Velocity = mMaxVelocity;
                return;
            }
            player.Velocity = MovementController.GetVelocity(player.Velocity, -mMaxVelocity / 50);
            if (player.Velocity < 0) player.Velocity = 0;
        }
    }
}
