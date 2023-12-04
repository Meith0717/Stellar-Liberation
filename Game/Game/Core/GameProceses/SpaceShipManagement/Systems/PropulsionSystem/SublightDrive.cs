// SublightEngine.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.Debugger;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using System;
using System.Runtime.InteropServices;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem
{
    public class SublightDrive
    {
        public bool IsMoving { get; private set; }

        public float MaxVelocity { get; }
        private float mVelocity;
        private readonly float Maneuverability;

        private Vector2? mDirection;
        private SpaceShip mShipTarget;

        public SublightDrive(float maxVelocity, float maneuverability)
        {
            MaxVelocity = maxVelocity;
            Maneuverability = maneuverability;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            var position = CollisionPredictor.PredictPosition(gameTime, spaceShip.Position, spaceShip.SublightEngine.MaxVelocity, mShipTarget);

            mDirection ??= (position is null) ? null : Vector2.Normalize((Vector2)position - spaceShip.Position);

            UpdateRotation(spaceShip);
            UpdateVelocity(spaceShip);
        }

        public void UpdateVelocity(SpaceShip spaceShip)
        {
            if (!IsMoving)
            {
                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, mVelocity / 100f);
                return;
            }

            if (mDirection is null) return;

            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)mDirection, 1));

            var relRotation = 1f - MathF.Abs(rotationUpdate) / MathF.PI;
            var rotationScore = MathF.Abs(0.5f - MathF.Abs(rotationUpdate) / MathF.PI);

            var targetVelocity = relRotation switch
            {
                < 0.7f => mVelocity * rotationScore,
                >= 0.7f => mVelocity * relRotation,
                float.NaN => 0
            };
            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, targetVelocity, mVelocity / 100f);
        }

        private void UpdateRotation(SpaceShip spaceShip)
        {
            if (mDirection is null) return;
            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)mDirection, 1)); spaceShip.Rotation += rotationUpdate * Maneuverability;
        }

        public void SetVelocity(float percentage) => mVelocity = MaxVelocity * percentage;

        public void MoveInDirection(Vector2 direction)
        {
            mShipTarget = null;
            mDirection = direction;
            IsMoving = true;
        }

        public void FollowSpaceShip(SpaceShip spaceShip)
        {
            mDirection = null;
            mShipTarget = spaceShip;
            IsMoving = true;
        }

        public void Standstill()
        {
            mShipTarget = null;
            mDirection = null;
            IsMoving = false;
        }

        public void ControlByInput(SpaceShip spaceShip, InputState inputState, Vector2 worldMousePosition)
        {
            inputState.DoAction(ActionType.Accelerate, () => spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, MaxVelocity, MaxVelocity / 100f));
            inputState.DoAction(ActionType.Break, () => spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, MaxVelocity / 100f));
            if (inputState.Actions.Contains(ActionType.RightClickHold)) return;
            MoveInDirection(Vector2.Normalize(worldMousePosition - spaceShip.Position));
        }

        public void Draw(DebugSystem debugSystem, SpaceShip spaceShip, Scene scene) => debugSystem.DrawMovingDir(mDirection, spaceShip, scene);
    }
}
