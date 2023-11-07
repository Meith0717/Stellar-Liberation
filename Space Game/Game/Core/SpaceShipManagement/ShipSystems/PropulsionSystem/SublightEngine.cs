// SublightEngine.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.Collision_Detection;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem
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

            IsMoving = mVector2Target.HasValue;

            UpdateRotation(spaceShip);
            UpdateVelocity(spaceShip);
        }

        private void UpdateVelocity(SpaceShip spaceShip)
        {
            if (mVector2Target == null)
            {
                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, .005f);
            }
            else
            {
                var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, mVector2Target.Value);
                var rotationScore = 1 - (MathF.Abs(rotationUpdate) / MathF.PI);

                spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, MaxVelocity * rotationScore, .005f);
            }
        }

        private void UpdateRotation(SpaceShip spaceShip)
        {
            if (mVector2Target != null)
            {
                var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, mVector2Target.Value);

               var rotationDirection = (rotationUpdate < 0) ? -1 : 1;

                spaceShip.Rotation += rotationDirection * Maneuverability;
            }
        }

        public void MoveToPosition(Vector2 position)
        {
            mShipTarget = null;
            mVector2Target = position;
        }

        public void FollowSpaceShip(SpaceShip spaceShip)
        {
            mVector2Target = null;
            mShipTarget = spaceShip;
        }

        public void Standstill()
        {
            mShipTarget = null;
            mVector2Target = null;
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
