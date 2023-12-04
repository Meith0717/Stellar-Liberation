// Camera2D.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.Utilitys;
using System;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    /// <summary>
    /// Represents a camera used for viewing and manipulating the game world.
    /// </summary>
    public class Camera2D
    {
        private const int CameraGlide = 100;
        public Vector2 Position { get; private set; }
        public float MaxZoom { get; private set; }
        public float MinZoom { get; private set; }

        private bool mAllowMovingWithMouse;

        /// <summary>
        /// Current zoom level of the camera.
        /// </summary>
        public float Zoom { get; set; }

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
        private Vector2 mLastScreenMousePosition;

        private bool mShake;
        private float mShakeAmount;


        public Camera2D(float minZoom, float maxZoom, bool allowMovingWithMouse)
        {
            Position = Vector2.Zero;
            MaxZoom = maxZoom;
            MinZoom = minZoom;
            mAllowMovingWithMouse = allowMovingWithMouse;
            Zoom = MaxZoom;
        }

        public void Shake(int amount)
        {
            mShake = true;
            mShakeAmount += amount;
        }

        private void MovingAnimation(GameTime gameTime)
        {
            mTargetPosition = Vector2.Distance(Position, mTargetPosition) < 10 ? Position : mTargetPosition;
            if (Position != mTargetPosition)
            {
                Position += (mTargetPosition - Position) / CameraGlide * gameTime.ElapsedGameTime.Milliseconds;
                return;
            }
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
            if (inputState.HasAction(ActionType.LeftClickHold))
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
            var multiplier = 1;

            inputState.DoAction(ActionType.CameraZoomIn, () => zoom += 5);
            inputState.DoAction(ActionType.CameraZoomOut, () => zoom -= 5);
            inputState.DoAction(ActionType.CtrlLeft, () => multiplier = 2 );
            mZoomAnimation = false;
            Zoom *= 1 + (zoom * multiplier) * 0.001f * gameTime.ElapsedGameTime.Milliseconds;
            Zoom = MathHelper.Clamp(Zoom, MinZoom, MaxZoom);
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
            mTargetPosition = position;
        }

        public void SetPosition(Vector2 position)
        {
            if (MovedByUser) return;
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
            MovingAnimation(gameTime);
            ZoomAnimation();
            Movement = Position - mLastPosition;
            mLastPosition = Position;

            if (!mShake) return;
            ExtendetRandom.Random.NextUnitVector(out var vec);
            Position += vec * mShakeAmount;
            mShakeAmount *= 0.9f;
            if (mShakeAmount > 0.1f) return;
            mShake = false;
            mShakeAmount = 0;
        }
    }
}