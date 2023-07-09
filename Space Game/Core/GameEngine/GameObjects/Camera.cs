/*
 *  Camera.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using GalaxyExplovive.Core.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using System;

namespace GalaxyExplovive.Core.GameEngine.GameObjects
{
    /// <summary>
    /// Represents a camera used for viewing and manipulating the game world.
    /// </summary>
    public class Camera
    {
        private const float MaxZoom = 0.001f;
        private const float MimZoom = 1f;

        /// <summary>
        /// Current zoom level of the camera.
        /// </summary>
        public float Zoom { get; private set; } = 1f;

        /// <summary>
        /// Current position of the camera.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Current movement vector of the camera.
        /// </summary>
        public Vector2 Movement { get; private set; }

        /// <summary>
        /// Indicates whether the camera has been moved by the user.
        /// </summary>
        public bool MovedByUser { get; private set; }

        private Vector2 mTargetPosition;
        private float mTargetZoom;
        private bool mZoomAnimation;
        private Vector2 mLastMousePosition;

        public Camera()
        {
            mZoomAnimation = false;
            Position = Vector2.Zero;
        }

        private void MovingAnimation(int spongy)
        {
            mTargetPosition = (Vector2.Distance(Position, mTargetPosition) < 0.1) ? Position : mTargetPosition;
            if (Position == mTargetPosition)  return;
            var adjustmentVector = mTargetPosition - Position;
            Movement = adjustmentVector / spongy;
            Position += Movement;
        }

        private void ZoomAnimation()
        {
            if (!mZoomAnimation) { return; }
            var zoomUpdate = -((Zoom - mTargetZoom) / 10);
            if (Math.Abs(zoomUpdate) > 0.0001f) { Zoom += zoomUpdate; return; }
            mZoomAnimation = false;
        }

        private void MoveCameraByMouse(InputState inputState, Vector2 mousePosition)
        {
            MovedByUser = false;
            Movement = Vector2.Zero;
            if (inputState.mMouseActionType != MouseActionType.LeftClickHold)
            {
                mLastMousePosition = mousePosition;
                return;
            }
            if (mLastMousePosition == mousePosition) return;
            Movement = mLastMousePosition - mousePosition;
            mTargetPosition += Movement;
            MovedByUser = true;
            mZoomAnimation = false;
            mLastMousePosition = mousePosition;
        }

        private void AdjustZoom(GameTime gameTime, InputState inputState)
        {
            var zoom = 0;
            if (inputState.mActionList.Contains(ActionType.CameraZoomIn) && Zoom + 0.0041 < MimZoom) { zoom += 7; }
            if (inputState.mActionList.Contains(ActionType.CameraZoomOut) && Zoom - 0.0041 > MaxZoom) { zoom -= 7; }
            if (zoom == 0) return;
            mZoomAnimation = false;
            Zoom *= 1 + zoom * 0.001f * gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// Sets the target zoom level for the camera.
        /// </summary>
        /// <param name="zoom">The target zoom level.</param>
        public void SetZoom(float zoom)
        {
            mTargetZoom = zoom;
            mZoomAnimation = true;
        }

        /// <summary>
        /// Sets the target position for the camera.
        /// </summary>
        /// <param name="position">The target position.</param>
        public void SetTarget(Vector2 position)
        {
            mTargetPosition = position;
        }

        /// <summary>
        /// Updates the camera based on the current game state.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">The input state.</param>
        /// <param name="mousePosition">The current mouse position.</param>
        public void Update(GameTime gameTime, InputState inputState, Vector2 mousePosition)
        {
            AdjustZoom(gameTime, inputState);
            MoveCameraByMouse(inputState, mousePosition);
            MovingAnimation(2);
            ZoomAnimation();
        }
    }
}