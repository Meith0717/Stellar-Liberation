using Microsoft.Xna.Framework;
using Galaxy_Explovive.Core.InputManagement;
using System;

namespace Galaxy_Explovive.Core.GameObject;
public class Camera2d
{
    public float mMaxZoom = 0.00000001f;
    public float mMimZoom = 1f;

    public float mZoom = 1f;
    public float mTargetZoom;
    public Vector2 mPosition;
    public Vector2 mTargetPosition;

    private int mWidth;
    private int mHeight;

    private bool mZoomAnimation;
    private Vector2 mLastMousePosition;

    public bool mIsMoving;

    // matrix variables
    private Matrix mTransform = Matrix.Identity;
    private bool mViewTransformationMatrixChanged = true;

    // animation stuff
    private float[] mAnimationX;
    private float[] mAnimationY;
    private int mAnimationIndex;
    private Vector2 mPositionBeforeAnimation;

    public Camera2d(int width, int height)
    {
        mZoomAnimation = false;
        mWidth = width;
        mHeight = height;
        mPosition = new Vector2(mWidth / 2f, mHeight / 2f);
        mIsMoving = false;
    }

    public void MoveAnimation(GameTime gameTime)
    {
        Vector2 adjustmentVector = Vector2.Subtract(mTargetPosition, mPosition);
        float distance = adjustmentVector.Length();
        adjustmentVector.Normalize();
        mPosition += adjustmentVector * distance / 10;
        SetPosition(mPosition);
    }

    public void ZoomAnimation(GameTime gameTime)
    {
        if (!mZoomAnimation) return;
        if (Math.Abs(mZoom - mTargetZoom) <= 0.01)
        {
            mZoom = mTargetZoom;
            mZoomAnimation = false;
        }
        if (mZoom < mTargetZoom)
        {
            mZoom += 0.01f;
        }
    }

    public void SetPosition(Vector2 position)
    {
        mPosition = position;
        mViewTransformationMatrixChanged = true;
    }

    private void MoveCameraByMouse(InputState inputState)
    {
        Vector2 currentMousePosition = inputState.mMousePosition.ToVector2();

        if (inputState.mMouseActionType != MouseActionType.RightClickHold)
        {
            mLastMousePosition = currentMousePosition;
            return;
        }

        if (mLastMousePosition != currentMousePosition)
        {
            Vector2 movement = ViewToWorld(mLastMousePosition) - ViewToWorld(currentMousePosition);
            mTargetPosition += movement;
            mIsMoving = true;
            mLastMousePosition = currentMousePosition;
        }
    }

    private void AdjustZoom(GameTime gameTime, InputState inputState)
    {
        // adjust zoom
        int zoom = 0;
        if (inputState.mActionList.Contains(ActionType.CameraZoomIn) && mZoom + 0.0041 < mMimZoom)
        {
            zoom += 7;
        }
        if (inputState.mActionList.Contains(ActionType.CameraZoomOut) && mZoom - 0.0041 > mMaxZoom)
        {
            zoom -= 7;
        }
        if (zoom != 0)
        {
            Zoom(1 + zoom * 0.001f * gameTime.ElapsedGameTime.Milliseconds);
        }
    }

    public void SetZoom(float zoom)
    {
        mTargetZoom = zoom;
        mZoomAnimation = true;
    }

    private void Zoom(float zoom)
    {
        mZoom *= zoom;
        mViewTransformationMatrixChanged = true;
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
            mTransform = Matrix.CreateTranslation(new Vector3(-mPosition.X, -mPosition.Y, 0))
                         * Matrix.CreateScale(mZoom, mZoom, 1)
                         * Matrix.CreateTranslation(new Vector3(mWidth / 2f, mHeight / 2f, 0));
            mViewTransformationMatrixChanged = false;
        }
        return mTransform;
    }

    public void Update(GameTime gameTime, InputState inputState)
    {
        mIsMoving = false;
        AdjustZoom(gameTime, inputState);
        MoveCameraByMouse(inputState);
        MoveAnimation(gameTime);
        ZoomAnimation(gameTime);
        // play animation if there is one
        if (mAnimationX != null)
        {
            if (mAnimationIndex < mAnimationX.Length)
            {
                mPosition = new Vector2(mAnimationX[mAnimationIndex], mAnimationY[mAnimationIndex]) + mPositionBeforeAnimation;
                mAnimationIndex++;
            }
            else
            {
                mPosition = mPositionBeforeAnimation;
                mAnimationIndex = 0;
                mAnimationX = null;
                mAnimationY = null;
            }
        }

        mViewTransformationMatrixChanged = true;
    }

    public void SetResolution(int width, int height)
    {
        mWidth = width;
        mHeight = height;
        mViewTransformationMatrixChanged = true;
    }

    public void Shake(int length = 20, int radiusStart = 100, int radiusEnd = 30)
    {
        if (mAnimationX == null)
        {
            mPositionBeforeAnimation = mPosition;
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
