using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
{
    public class SublightEngine
    {
        private float mMaxVelocity;
        private Vector2? mTarget;

        public SublightEngine(float maxVelocity)
        {
            mMaxVelocity = maxVelocity;
        }

        public void Update(SpaceShip spaceShip)
        {
            switch (mTarget)
            {
                case null:
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, -1f);
                    if (spaceShip.Velocity < 0) spaceShip.Velocity = 0;
                    break;
                case not null:
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mTarget, 0.1f);
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 1f);
                    if (spaceShip.Velocity > mMaxVelocity) spaceShip.Velocity = mMaxVelocity;
                    if (Vector2.Distance((Vector2)mTarget, spaceShip.Position) < 10000) mTarget = null;
                    break;
            }
        }

        public void SetTarget(Vector2? target) => mTarget = target;

        public void FollowMouse(InputState inputState, SpaceShip spaceShip, Vector2 worldMousePosition)
        {
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, worldMousePosition, .1f);
            if (inputState.HasMouseAction(MouseActionType.RightClickHold))
            {
                SetTarget(worldMousePosition);
                return;
            }
            SetTarget(null);
        }
    }
}
