


/*
 *  Camera.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Utility;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.GameObjects
{
    /// <summary>
    /// Represents a camera used for viewing and manipulating the game world.
    /// </summary>
    public class Camera
    {
        private const int CameraGlide = 4;
        private float mMaxZoom;
        private float mMinZoom;
        private bool mAllowMovingWithMouse;

        /// <summary>
        /// Current zoom level of the camera.
        /// </summary>
        public float Zoom { get; private set; }

        /// <summary>
        /// Current position of the camera.
        /// </summary>
        public Vector2 Position { get; private set; }
        private Vector2 mLastPosition;

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
        private bool mMoveAnimation;
        private Vector2 mLastScreenMousePosition;

        public Camera(float minZoom, float maxZoom, bool allowMovingWithMouse)
        {
            Position = Vector2.Zero;
            mMaxZoom = maxZoom;
            mMinZoom = minZoom;
            mAllowMovingWithMouse = allowMovingWithMouse;
            Zoom = mMinZoom;
        }

        private void MovingAnimation()
        {
            mTargetPosition = Vector2.Distance(Position, mTargetPosition) < 1 ? Position : mTargetPosition;
            if (Position != mTargetPosition)
            {
                mMoveAnimation = true;
                Position += (mTargetPosition - Position) / CameraGlide; ;
                return;
            }
            mMoveAnimation = false;
        }

        private void ZoomAnimation()
        {
            if (mZoomAnimation)
            {
                var zoomUpdate = -((Zoom - mTargetZoom) / 10);
                if (Math.Abs(zoomUpdate) > 0.0001f) { Zoom += zoomUpdate; return; }
                mZoomAnimation = false;
            }
        }

        private void MoveCameraByMouse(InputState inputState, Vector2 screenMousePosition, Matrix ViewTransformation)
        {
            MovedByUser = false;
            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                if (Vector2.Distance(mLastScreenMousePosition, screenMousePosition) > 1)
                {
                    MovedByUser = true;
                    var lastWorldMousePosition = Transformations.ScreenToWorld(ViewTransformation, mLastScreenMousePosition);
                    var worldMousePosition = Transformations.ScreenToWorld(ViewTransformation, screenMousePosition);
                    mTargetPosition += lastWorldMousePosition - worldMousePosition;
                }
            };
            mLastScreenMousePosition = screenMousePosition;
        }

        private void AdjustZoomByMouse(GameTime gameTime, InputState inputState)
        {
            var zoom = 0f;
            if (inputState.HasAction(ActionType.CameraZoomIn) && Zoom < mMaxZoom)
            {
                zoom += 5f * inputState.mGamePadValues.mRightThumbSticks.Y == 0 ? 10 : inputState.mGamePadValues.mRightThumbSticks.Y;
            }
            if (inputState.HasAction(ActionType.CameraZoomOut) && Zoom > mMinZoom)
            {
                zoom += 5f * inputState.mGamePadValues.mRightThumbSticks.Y == 0 ? -10 : inputState.mGamePadValues.mRightThumbSticks.Y;
            }

            if (zoom == 0) return;
            mZoomAnimation = false;
            Zoom *= 1 + zoom * 0.001f * gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// Sets the target zoom level for the camera.
        /// </summary>
        /// <param name="zoom">The target zoom level.</param>
        public void MoveToZoom(float zoom)
        {
            mTargetZoom = zoom;
            mZoomAnimation = true;
        }

        /// <summary>
        /// Sets the target position for the camera.
        /// </summary>
        /// <param name="position">The target position.</param>
        public void MoveToTarget(Vector2 position)
        {
            if (MovedByUser) return;
            mMoveAnimation = true;
            mTargetPosition = position;
        }

        public void SetPosition(Vector2 position)
        {
            if (MovedByUser || mMoveAnimation) return;
            Position = mTargetPosition = position;
        }

        /// <summary>
        /// Updates the camera based on the current game state.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">The input state.</param>
        /// <param name="mousePosition">The current mouse position.</param>
        public void Update(GameTime gameTime, InputState inputState, Vector2 screenMousePosition, Matrix ViewTransformation)
        {
            Movement = Vector2.Zero;
            AdjustZoomByMouse(gameTime, inputState);
            if (mAllowMovingWithMouse) MoveCameraByMouse(inputState, screenMousePosition, ViewTransformation);
            MovingAnimation();
            ZoomAnimation();
            Movement = Position - mLastPosition;
            mLastPosition = Position;
        }
    }
}