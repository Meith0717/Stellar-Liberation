using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Core.GameObject;
public class Camera2d
{   
    // Constants
    const float mMaxZoom = 0.00000001f;
    const float mMimZoom = 1.2f;

    public float Zoom { get; private set; } = 1f;
    public Vector2 Position { get; private set; }
    public Vector2 TargetPosition { private get; set; }
    public Vector2 Movement { get; private set; }
    public float mTargetZoom;
    public bool mIsMoving;

    // matrix variables
    private Matrix mTransform = Matrix.Identity;
    private bool mViewTransformationMatrixChanged = true;
    private GraphicsDevice mGraphicsDevice;

    // animation stuff
    private bool mZoomAnimation;
    private Vector2 mLastMousePosition;
    private float[] mAnimationX;
    private float[] mAnimationY;
    private int mAnimationIndex;
    private Vector2 mPositionBeforeAnimation;

    public Camera2d(GraphicsDevice graphicsDevice)
    {
        mZoomAnimation = false;
        mGraphicsDevice = graphicsDevice;
        int width = mGraphicsDevice.Viewport.Width;
        int height = mGraphicsDevice.Viewport.Height;
        Position = new Vector2(width / 2f, height / 2f);
        mIsMoving = false;
    }

    private void MovingAnimation(int spongy)
    {
        if (Position == TargetPosition) { return; }
        Vector2 adjustmentVector = Vector2.Subtract(TargetPosition, Position);
        Position += adjustmentVector / spongy;
    }

    public void ZoomAnimation()
    {
        if (!mZoomAnimation) { return; }
        float zoomUpdate = - ((Zoom - mTargetZoom)/10);
        if (Math.Abs(zoomUpdate) > 0.0001f) { Zoom += zoomUpdate; return; }
        mZoomAnimation = false;
    }

    private void MoveCameraByMouse(InputState inputState)
    {
        Vector2 currentMousePosition = inputState.mMousePosition.ToVector2();
        mIsMoving = false;
        Movement = Vector2.Zero;

        if (inputState.mMouseActionType != MouseActionType.LeftClickHold)
        {
            mLastMousePosition = currentMousePosition;
            return;
        }

        if (mLastMousePosition != currentMousePosition)
        {
            Movement = ViewToWorld(mLastMousePosition) - ViewToWorld(currentMousePosition);
            TargetPosition += Movement;
            mIsMoving = true;
            mZoomAnimation = false;
            mLastMousePosition = currentMousePosition;
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
            Zoom *= (1 + zoom * 0.001f * gameTime.ElapsedGameTime.Milliseconds);
            mViewTransformationMatrixChanged = true;
        }
    }

    public void SetZoom(float zoom)
    {
        mTargetZoom = zoom;
        mZoomAnimation = true;
    }

    // ReSharper disable once UnusedMember.Global
    public Vector2 WorldToView(Vector2 vector)
    {
        return Vector2.Transform(vector, mTransform);
    }

    public Vector2 ViewToWorld(Vector2 vector)
    {
        return Vector2.Transform(vector, Matrix.Invert(mTransform));
    }

    public Matrix GetViewTransformationMatrix()
    {
        if (mViewTransformationMatrixChanged)
        {
            int width = mGraphicsDevice.Viewport.Width;
            int height = mGraphicsDevice.Viewport.Height;
            mTransform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0))
                         * Matrix.CreateScale(Zoom, Zoom, 1)
                         * Matrix.CreateTranslation(new Vector3(width / 2f, height / 2f, 0));
            mViewTransformationMatrixChanged = false;
        }
        return mTransform;
    }

    public void Update(GameTime gameTime, InputState inputState)
    {
        AdjustZoom(gameTime, inputState);
        MoveCameraByMouse(inputState);
        MovingAnimation(2);
        ZoomAnimation();
        // play animation if there is one
        if (mAnimationX != null)
        {
            if (mAnimationIndex < mAnimationX.Length)
            {
                Position = new Vector2(mAnimationX[mAnimationIndex], mAnimationY[mAnimationIndex]) + mPositionBeforeAnimation;
                mAnimationIndex++;
            }
            else
            {
                Position = mPositionBeforeAnimation;
                mAnimationIndex = 0;
                mAnimationX = null;
                mAnimationY = null;
            }
        }

        mViewTransformationMatrixChanged = true;
    }

    public void Shake(int length = 20, int radiusStart = 100, int radiusEnd = 30)
    {
        if (mAnimationX == null)
        {
            mPositionBeforeAnimation = Position;
        }
        Random random = new Random();
        float[] graphX = new float[length];
        float[] graphY = new float[length];

        int radius = radiusStart;
        for (int i = 0; i < length; i++)
        {
            radius -= (radiusStart - radiusEnd) / length;
            graphX[i] = (int)(random.NextDouble() * radius - radius / 2f);
            graphY[i] = (int)(random.NextDouble() * radius - radius / 2f);
        }

        mAnimationX = graphX;
        mAnimationY = graphY;
    }
}
