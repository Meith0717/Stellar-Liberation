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

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem
{
    public class SublightDrive
    {
        public bool IsMoving { get; private set; }

        public float MaxVelocity { get; }
        private float mVelocity;
        private readonly float Maneuverability;

        private Vector2? mVector2Target;
        private SpaceShip mShipTarget;

        public SublightDrive(float maxVelocity, float maneuverability)
        {
            MaxVelocity = maxVelocity;
            Maneuverability = maneuverability;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            var position = CollisionPredictor.PredictPosition(gameTime, spaceShip.Position, spaceShip.SublightEngine.MaxVelocity, mShipTarget);

            mVector2Target ??= position;

            UpdateRotation(spaceShip);
        }

        public void UpdateVelocity(SpaceShip spaceShip)
        {
            if (!IsMoving)
            {
                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, mVelocity / 100f);
                return;
            }

            if (mVector2Target is null) return;

            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, mVector2Target.Value);

            var relRotation = 1f - MathF.Abs(rotationUpdate) / MathF.PI;
            var rotationScore = MathF.Abs(0.5f - MathF.Abs(rotationUpdate) / MathF.PI);

            var targetVelocity = relRotation switch
            {
                < 0.7f => mVelocity * rotationScore,
                >= 0.7f => mVelocity * relRotation,
                float.NaN => 0
            };
            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, targetVelocity, mVelocity / 100f);
            if (spaceShip.BoundedBox.Contains((Vector2)mVector2Target))
                Standstill();
        }

        private void UpdateRotation(SpaceShip spaceShip)
        {
            if (mVector2Target != null)
            {
                var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, mVector2Target.Value);

                spaceShip.Rotation += rotationUpdate * Maneuverability;
            }
        }

        public void SetVelocity(float percentage) => mVelocity = MaxVelocity * percentage;

        public void MoveToPosition(Vector2 position)
        {
            mShipTarget = null;
            mVector2Target = position;
            IsMoving = true;
        }

        public void FollowSpaceShip(SpaceShip spaceShip)
        {
            mVector2Target = null;
            mShipTarget = spaceShip;
            IsMoving = true;
        }

        public void Standstill()
        {
            mShipTarget = null;
            mVector2Target = null;
            IsMoving = false;
        }

        public void ControlByInput(SpaceShip spaceShip, InputState inputState, Vector2 worldMousePosition)
        {
            switch (inputState.GamePadIsConnected)
            {
                case true:
                    inputState.DoAction(ActionType.Accelerate, () => spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, MaxVelocity, MaxVelocity / 100f * inputState.mThumbSticksState.RightTrigger));
                    inputState.DoAction(ActionType.Break, () => spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, MaxVelocity / 100f * inputState.mThumbSticksState.LeftTrigger));

                    var leftThumbSticksValue = inputState.mThumbSticksState.LeftThumbSticks;
                    if (leftThumbSticksValue == Vector2.Zero) break;
                    var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, Geometry.AngleBetweenVectors(Vector2.Zero, leftThumbSticksValue), 1);
                    spaceShip.Rotation += rotationUpdate * Maneuverability;
                    break;
                case false:
                    inputState.DoAction(ActionType.Accelerate, () => spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, MaxVelocity, MaxVelocity / 100f));
                    inputState.DoAction(ActionType.Break, () => spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, MaxVelocity / 100f));
                    MoveToPosition(worldMousePosition);
                    break;
            }
        }

        public void Draw(DebugSystem debugSystem, SpaceShip spaceShip, Scene scene) => debugSystem.DrawMovingDir(mVector2Target, spaceShip, scene);
    }
}
