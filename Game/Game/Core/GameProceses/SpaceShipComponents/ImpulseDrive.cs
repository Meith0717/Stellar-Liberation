// ImpulseDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents
{
    [Serializable]
    public class ImpulseDrive
    {
        private const float Maneuverability = 0.01f;

        [JsonProperty] public readonly float MaxVelocity;
        [JsonProperty] private Vector2? mVectorTarget;
        [JsonProperty] private Spaceship mShipTarget;
        [JsonProperty] private Vector2? mDirectionTarget;

        [JsonIgnore] public bool IsMoving { get; private set; }

        public ImpulseDrive(float velocityPerc) => MaxVelocity = 1 * velocityPerc;

        public void Update(GameTime gameTime, Spaceship spaceShip, double damage)
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
                    direction = spaceShip.Position.DirectionToVector2((Vector2)targetPosition);
                    IsMoving = IsMoving && !spaceShip.BoundedBox.Contains((Vector2)targetPosition);
                    break;
            }

            UpdateRotation(gameTime, spaceShip, direction);
            UpdateVelocity(gameTime, spaceShip, direction);
            spaceShip.Velocity *= (float)damage;
        }

        #region Moving Logic
        private void UpdateRotation(GameTime gameTime, Spaceship spaceShip, Vector2? direction)
        {
            if (direction is null) return;
            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)direction, 1));
            spaceShip.Rotation += rotationUpdate * Maneuverability * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        private void UpdateVelocity(GameTime gameTime, Spaceship spaceShip, Vector2? direction)
        {
            if (!IsMoving)
            {
                spaceShip.Velocity = MovementController.GetVelocity(gameTime, spaceShip.Velocity, 0, MaxVelocity / 100f);
                return;
            }

            if (direction is null) return;

            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, Geometry.GetPointInDirection(spaceShip.Position, (Vector2)direction, 1));

            var relRotation = 1f - MathF.Abs(rotationUpdate) / MathF.PI;
            var rotationScore = MathF.Abs(0.5f - MathF.Abs(rotationUpdate) / MathF.PI);

            var targetVelocity = relRotation switch
            {
                < 0.7f => MaxVelocity * rotationScore,
                >= 0.7f => MaxVelocity * relRotation,
                float.NaN => 0
            };

            spaceShip.Velocity = MovementController.GetVelocity(gameTime, spaceShip.Velocity, targetVelocity, MaxVelocity / 100f);
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

        public void FollowSpaceship(Spaceship spaceShip)
        {
            mDirectionTarget = null;
            mVectorTarget = null;
            mShipTarget = spaceShip;
            IsMoving = true;
        }
        #endregion

    }
}
