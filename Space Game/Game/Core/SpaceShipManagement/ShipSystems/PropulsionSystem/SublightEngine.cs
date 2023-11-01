// SublightEngine.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem
{
    public class SublightEngine
    {
        public bool IsMoving { get; private set; }

        // Atributes
        private float mMaxVelocity;
        private float mManeuverability;

        // Targets
        private Vector2? mVector2Target;
        private SpaceShip mShipTarget;

        public SublightEngine(float maxVelocity, float maneuverability)
        {
            mMaxVelocity = maxVelocity;
            mManeuverability = maneuverability;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            mVector2Target ??= mShipTarget?.Position;

            switch (mVector2Target)
            {
                case null:
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, -.05f);
                    if (spaceShip.Velocity < 0) spaceShip.Velocity = 0;
                    IsMoving = false;
                    break;
                case not null:
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mVector2Target, mManeuverability);
                    spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, .05f);
                    if (spaceShip.Velocity > mMaxVelocity) spaceShip.Velocity = mMaxVelocity;
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

        public void Draw(SpaceShip spaceShip, Scene scene)
        {
            if (mVector2Target is null) return;
            TextureManager.Instance.DrawAdaptiveLine(spaceShip.Position, (Vector2)mVector2Target, Color.Black, 1, spaceShip.TextureDepth -1, scene.Camera.Zoom);
        }
    }
}
