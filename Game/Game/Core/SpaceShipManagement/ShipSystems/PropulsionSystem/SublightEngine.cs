// SublightEngine.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using System;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem
{
    public class SublightEngine
    {
        public bool IsMoving { get; private set; }
        public float MaxVelocity { get; }
        private readonly float Maneuverability;

        private Vector2? mVector2Target;
        private SpaceShip mShipTarget;

        public SublightEngine(float maxVelocity, float maneuverability)
        {
            MaxVelocity = maxVelocity;
            Maneuverability = maneuverability;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            var position = CollisionPredictor.PredictPosition(gameTime, spaceShip.Position, spaceShip.SublightEngine.MaxVelocity, mShipTarget);

            mVector2Target ??= position;

            UpdateRotation(spaceShip);
            UpdateVelocity(spaceShip);
        }

        private void UpdateVelocity(SpaceShip spaceShip)
        {
            if (!IsMoving)
            {
                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, .005f);
                return;
            }

            if (mVector2Target is null) return;

            var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, mVector2Target.Value);

            var relRotation = 1 - (MathF.Abs(rotationUpdate) / MathF.PI);
            var rotationScore = MathF.Abs(0.5f - (MathF.Abs(rotationUpdate) / MathF.PI));

            var targetVelocity = relRotation switch
            {
                < 0.7f => MaxVelocity * rotationScore,
                >= 0.7f => MaxVelocity * relRotation,
                float.NaN => 0
            };
            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, targetVelocity, .005f);
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

        public void FollowMouse(InputState inputState, Vector2 worldMousePosition)
        {
            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                MoveToPosition(worldMousePosition);
            }
            else
            {
                Standstill();
            }
        }

        public void Draw(Debugger.DebugSystem debugSystem, SpaceShip spaceShip, Scene scene)
        {
            debugSystem.DrawPath(mVector2Target, spaceShip, scene);
        }
    }

}
