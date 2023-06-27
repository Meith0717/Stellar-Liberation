using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;
using System;

namespace Galaxy_Explovive.Core.GameObjects
{
    public class Camera
    {
        // Constants
        const float mMaxZoom = 0.001f;
        const float mMimZoom = 1f;

        public float Zoom { get; private set; } = 1f;
        public Vector2 Position { get; private set; }
        public Vector2 Movement { get; private set; }
        public bool MovedByUser { get; private set; } = false;

        // animation stuff
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
            Position = Vector2.Distance(Position, mTargetPosition) < 0.1 ? mTargetPosition : Position;
            if (Position == mTargetPosition) { return; }
            Vector2 adjustmentVector = Vector2.Subtract(mTargetPosition, Position);
            Movement = adjustmentVector / spongy;
            Position += Movement;
        }

        private void ZoomAnimation()
        {
            if (!mZoomAnimation) { return; }
            float zoomUpdate = -((Zoom - mTargetZoom) / 10);
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

            if (mLastMousePosition != mousePosition)
            {
                Movement =  mLastMousePosition - mousePosition;
                mTargetPosition += Movement;
                MovedByUser = true;
                mZoomAnimation = false;
                mLastMousePosition = mousePosition;
            }
        }

        private void AdjustZoom(GameTime gameTime, InputState inputState)
        {
            // adjust zoom
            int zoom = 0;
            if (inputState.mActionList.Contains(ActionType.CameraZoomIn) && Zoom + 0.0041 < mMimZoom) { zoom += 7; }
            if (inputState.mActionList.Contains(ActionType.CameraZoomOut) && Zoom - 0.0041 > mMaxZoom) { zoom -= 7; }
            if (zoom != 0)
            {
                mZoomAnimation = false;
                Zoom *= 1 + zoom * 0.001f * gameTime.ElapsedGameTime.Milliseconds;            }
        }

        public void SetZoom(float zoom)
        {
            mTargetZoom = zoom;
            mZoomAnimation = true;
        } 

        public void SetTarget(Vector2 position)
        {
            mTargetPosition = position;
        }

        public void Update(GameTime gameTime, InputState inputState, Vector2 mousePosition)
        {
            AdjustZoom(gameTime, inputState);
            MoveCameraByMouse(inputState, mousePosition);
            MovingAnimation(2);
            ZoomAnimation();
        }
    }
}

