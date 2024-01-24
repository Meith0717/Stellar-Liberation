// SublightDrive.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.Debugger;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem
{
    public class SublightDrive
    {
        public bool IsMoving { get; private set; }

        public float MaxVelocity { get; }
        private float mTargetVelocity;
        private readonly float Maneuverability;

        private Vector2? mDirection;
        private Vector2? mVectorTarget;
        private SpaceShip mShipTarget;

        public SublightDrive(float maxVelocity, float maneuverability)
        {
            MaxVelocity = maxVelocity;
            Maneuverability = maneuverability;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            var position = mVectorTarget ?? CollisionPredictor.PredictPosition(gameTime, spaceShip.Position, spaceShip.SublightEngine.MaxVelocity, mShipTarget);

            mDirection ??= (position is null) ? null : Vector2.Normalize((Vector2)position - spaceShip.Position);

            UpdateRotation(spaceShip);
            UpdateVelocity(spaceShip);

            if (mVectorTarget is null) return;
            if (Vector2.Distance((Vector2)mVectorTarget, spaceShip.Position) > 1000) return;
            Standstill();
            SetVelocity(0);
        }

        public void UpdateVelocity(SpaceShip spaceShip)
        {
            if (!IsMoving)
            {
                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, MaxVelocity / 100f);
                return;
            }

            if (mDirection is null) return;

            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)mDirection, 1));

            var relRotation = 1f - MathF.Abs(rotationUpdate) / MathF.PI;
            var rotationScore = MathF.Abs(0.5f - MathF.Abs(rotationUpdate) / MathF.PI);

            var targetVelocity = relRotation switch
            {
                < 0.7f => mTargetVelocity * rotationScore,
                >= 0.7f => mTargetVelocity * relRotation,
                float.NaN => 0
            };
            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, targetVelocity, MaxVelocity / 100f);
        }

        private void UpdateRotation(SpaceShip spaceShip)
        {
            if (mDirection is null) return;
            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)mDirection, 1)); spaceShip.Rotation += rotationUpdate * Maneuverability;
        }

        public void SetVelocity(float percentage) => mTargetVelocity = MaxVelocity * percentage;

        public void MoveInDirection(Vector2 direction)
        {
            mShipTarget = null;
            mDirection = direction;
            mVectorTarget = null;
            IsMoving = true;
        }

        public void MoveToTarget(Vector2 target)
        {
            mShipTarget = null;
            mVectorTarget = target;
            mDirection = null;
            IsMoving = true;
        }

        public void FollowSpaceShip(SpaceShip spaceShip)
        {
            mDirection = null;
            mVectorTarget = null;
            mShipTarget = spaceShip;
            IsMoving = true;
        }

        public void Standstill()
        {
            mShipTarget = null;
            mVectorTarget = null;
            mDirection = null;
            IsMoving = false;
        }

        public void ControlByInput(SpaceShip spaceShip, InputState inputState, Vector2 worldMousePosition)
        {
            inputState.DoAction(ActionType.Accelerate, () => mTargetVelocity = MathHelper.Clamp(mTargetVelocity + .1f, 0, MaxVelocity));
            inputState.DoAction(ActionType.Break, () => mTargetVelocity = MathHelper.Clamp(mTargetVelocity - .1f, 0, MaxVelocity));

            MoveInDirection(Vector2.Normalize(worldMousePosition - spaceShip.Position));
        }

        public void Draw(DebugSystem debugSystem, SpaceShip spaceShip, Scene scene) => debugSystem.DrawMovingDir(mDirection, spaceShip, scene);
    }
}
