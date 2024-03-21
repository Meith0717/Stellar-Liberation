// SublightDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.Debugging;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components.PropulsionSystem
{
    public class SublightDrive
    {
        public bool IsMoving { get; private set; }

        public float MaxVelocity { get; }
        private float mTargetVelocity;
        private readonly float Maneuverability;

        private Vector2? mDirection;
        private Vector2? mVectorTarget;
        private Spaceship mShipTarget;
        private Vector2? mDirectionTarget;

        public SublightDrive(float maxVelocity, float maneuverability)
        {
            MaxVelocity = maxVelocity;
            Maneuverability = maneuverability;
        }

        public void Update(GameTime gameTime, Spaceship spaceShip, double damage)
        {
            var position = mVectorTarget ?? mShipTarget?.Position;

            mDirection = (position is null) ? mDirectionTarget : Vector2.Normalize((Vector2)position - spaceShip.Position);

            UpdateRotation(gameTime, spaceShip);
            UpdateVelocity(gameTime, spaceShip, damage);

            if (mVectorTarget is null) return;
            if (Vector2.Distance((Vector2)mVectorTarget, spaceShip.Position) > 1000) return;
            Standstill();
            SetVelocity(0);
        }

        private void UpdateVelocity(GameTime gameTime, Spaceship spaceShip, double damage)
        {
            if (!IsMoving)
            {
                spaceShip.Velocity = MovementController.GetVelocity(gameTime, spaceShip.Velocity, 0, MaxVelocity / 100f);
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
            targetVelocity *= (float)damage;
            spaceShip.Velocity = MovementController.GetVelocity(gameTime, spaceShip.Velocity, targetVelocity, MaxVelocity / 100f);
        }

        private void UpdateRotation(GameTime gameTime, Spaceship spaceShip)
        {
            if (mDirection is null) return;
            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)mDirection, 1));
            spaceShip.Rotation += rotationUpdate * Maneuverability * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void SetVelocity(float percentage) => mTargetVelocity = MaxVelocity * percentage;

        public void MoveInDirection(Vector2 direction)
        {
            mShipTarget = null;
            mDirectionTarget = direction;
            mVectorTarget = null;
            IsMoving = true;
        }

        public void MoveToTarget(Vector2 target)
        {
            mShipTarget = null;
            mVectorTarget = target;
            mDirectionTarget = null;
            IsMoving = true;
        }

        public void FollowSpaceship(Spaceship spaceShip)
        {
            mDirectionTarget = null;
            mVectorTarget = null;
            mShipTarget = spaceShip;
            IsMoving = true;
        }

        public void Standstill()
        {
            mShipTarget = null;
            mVectorTarget = null;
            mDirectionTarget = null;
            IsMoving = false;
        }

        public void Draw(DebugSystem debugSystem, Spaceship spaceShip, GameLayer scene)
            => debugSystem.DrawMovingDir(mDirection, spaceShip, scene);
    }
}
