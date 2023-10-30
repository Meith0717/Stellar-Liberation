// SublightEngine.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem
{
    public class SublightEngine
    {
        public bool IsMoving { get; private set; }
        private float mMaxVelocity;
        private float mManeuverability;
        private Vector2? mTarget;

        public SublightEngine(float maxVelocity, float maneuverability)
        {
            mMaxVelocity = maxVelocity;
            mManeuverability = maneuverability;
        }

        public void Update(SpaceShip spaceShip)
        {
            switch (mTarget)
            {
                case null:
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, -.05f);
                    if (spaceShip.Velocity < 0) spaceShip.Velocity = 0;
                    IsMoving = false;
                    break;
                case not null:
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mTarget, mManeuverability);
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, .05f);
                    if (spaceShip.Velocity > mMaxVelocity) spaceShip.Velocity = mMaxVelocity;
                    SetTarget(spaceShip, mTarget);
                    IsMoving = true;
                    break;
            }
        }

        public bool IsTargetReached(SpaceShip spaceShip, Vector2? target)
        {
            if (target is null) return true;
            return Vector2.Distance((Vector2)target, spaceShip.Position) < 1000;
        }

        public bool SetTarget(SpaceShip spaceShip, Vector2? target)
        {
            if (IsTargetReached(spaceShip, target))
            {
                mTarget = null;
                return false;
            }
            mTarget = target;
            return true;
        }

        public void FollowMouse(InputState inputState, SpaceShip spaceShip, Vector2 worldMousePosition)
        {
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, worldMousePosition, .1f);
            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                SetTarget(spaceShip, worldMousePosition);
                return;
            }
            SetTarget(spaceShip, null);
        }

        public void Draw(SpaceShip spaceShip, Scene scene)
        {
            if (mTarget is null) return;
            TextureManager.Instance.DrawAdaptiveLine(spaceShip.Position, (Vector2)mTarget, new Color(20, 20, 20, 20), 1, spaceShip.TextureDepth -1, scene.Camera.Zoom);
        }
    }
}
