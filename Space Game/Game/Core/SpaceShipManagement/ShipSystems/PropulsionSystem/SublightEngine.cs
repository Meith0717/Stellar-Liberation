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

        // Atributes
        public float MaxVelocity { get; private set; }
        private float mManeuverability;

        // Targets
        private Vector2? mVector2Target;
        private SpaceShip mShipTarget;

        public SublightEngine(float maxVelocity, float maneuverability)
        {
            MaxVelocity = maxVelocity;
            mManeuverability = maneuverability;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            var position = CollisionPredictor.PredictPosition(gameTime, spaceShip.Position, spaceShip.SublightEngine.MaxVelocity, mShipTarget);
            mVector2Target ??= position;

            switch (mVector2Target)
            {
                case null:
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, -.05f);
                    if (spaceShip.Velocity < 0) spaceShip.Velocity = 0;
                    IsMoving = false;
                    break;
                case not null:
                    var rotationUpdate = MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mVector2Target, mManeuverability);
                    switch (MathF.Abs(rotationUpdate))
                    {
                        case > 0.012f:
                            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, -.003f);
                            break;
                        case <= 0.012f:
                            spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, .05f);
                            break;
                    }
                    spaceShip.Rotation += rotationUpdate;
                    if (spaceShip.Velocity > MaxVelocity) spaceShip.Velocity = MaxVelocity;
                    IsMoving = true;
                    if (spaceShip.BoundedBox.Contains((Vector2)mVector2Target)) ClearTarget();
                    break;
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

        public void ClearTarget()
        {
            mShipTarget = null;
            mVector2Target = null;
        }

        public void FollowMouse(InputState inputState, SpaceShip spaceShip, Vector2 worldMousePosition)
        {
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, worldMousePosition, mManeuverability);
            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                MoveToPosition(worldMousePosition);
                return;
            }
            ClearTarget();
        }

        public void Draw(Debugger.DebugSystem debugSystem, SpaceShip spaceShip, Scene scene) => debugSystem.DrawPath(mVector2Target, spaceShip, scene);
    }
}
