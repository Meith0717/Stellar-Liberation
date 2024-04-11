// ImpulseDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    [Serializable]
    public class ImpulseDrive
    {
        private const float Maneuverability = 0.01f;

        [JsonProperty] public float MaxVelocity { get; private set; }
        [JsonProperty] private Vector2? mVectorTarget;
        [JsonProperty] private Flagship mShipTarget;
        [JsonProperty] private Vector2? mDirectionTarget;

        [JsonProperty] public bool IsMoving { get; private set; }

        public ImpulseDrive(float velocity) => MaxVelocity = velocity;

        public void Boost(float velocityPerc) => MaxVelocity *= velocityPerc;

        public void Move(GameTime gameTime, GameObject obj, double damage)
        {
            var targetPosition = mVectorTarget ?? mShipTarget?.Position;

            Vector2? direction;
            switch (targetPosition)
            {
                case null:
                    direction = mDirectionTarget;
                    IsMoving = mDirectionTarget is not null;
                    break;
                case not null:
                    direction = obj.Position.DirectionToVector2((Vector2)targetPosition);
                    IsMoving = IsMoving && !obj.BoundedBox.Contains((Vector2)targetPosition);
                    break;
            }

            UpdateRotation(gameTime, obj, direction);
            UpdateVelocity(gameTime, obj, direction);
            obj.Velocity *= (float)damage;
        }

        #region Moving Logic
        private void UpdateRotation(GameTime gameTime, GameObject obj, Vector2? direction)
        {
            if (direction is null) return;
            var rotationUpdate = MovementController.GetRotationUpdate(obj.Rotation, obj.Position, Geometry.GetPointInDirection(obj.Position, (Vector2)direction, 1));
            obj.Rotation += rotationUpdate * Maneuverability * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        private void UpdateVelocity(GameTime gameTime, GameObject obj, Vector2? direction)
        {
            if (!IsMoving)
            {
                obj.Velocity = MovementController.GetVelocity(gameTime, obj.Velocity, 0, MaxVelocity / 100f);
                return;
            }

            if (direction is null) return;

            var rotationUpdate = MovementController.GetRotationUpdate(obj.Rotation, obj.Position, Geometry.GetPointInDirection(obj.Position, (Vector2)direction, 1));

            var relRotation = 1f - MathF.Abs(rotationUpdate) / MathF.PI;
            var rotationScore = MathF.Abs(0.5f - MathF.Abs(rotationUpdate) / MathF.PI);

            var targetVelocity = relRotation switch
            {
                < 0.7f => MaxVelocity * rotationScore,
                >= 0.7f => MaxVelocity * relRotation,
                float.NaN => 0
            };

            obj.Velocity = MovementController.GetVelocity(gameTime, obj.Velocity, targetVelocity, MaxVelocity / 100f);
        }
        #endregion

        #region Commands
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

        public void FollowSpaceship(Flagship spaceShip)
        {
            mDirectionTarget = null;
            mVectorTarget = null;
            mShipTarget = spaceShip;
            IsMoving = true;
        }
        #endregion

    }
}
