using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;

namespace rache_der_reti.Core.TextureManagement;
[Serializable]
public class Camera2d
{
    [JsonProperty] private float mZoom = 1.0f;
    [JsonProperty] private float mMaxZoom = 0.1f;
    [JsonProperty] private float mMimZoom = 3;
    [JsonProperty] public Vector2 mPosition;
    [JsonProperty] private int mWidth;
    [JsonProperty] private int mHeight;

    // matrix variables
    [JsonProperty] private Matrix mTransform = Matrix.Identity;
    [JsonProperty] private bool mViewTransformationMatrixChanged = true;

    // animation stuff
    [JsonProperty] private float[] mAnimationX;
    [JsonProperty] private float[] mAnimationY;
    [JsonProperty] private int mAnimationIndex;
    [JsonProperty] private Vector2 mPositionBeforeAnimation;

    public Camera2d(int width, int height)
    {
        mWidth = width;
        mHeight = height;
        mPosition = new Vector2(mWidth / 2f, mHeight / 2f);
    }

    public void SetPosition(Vector2 position)
    {
        mPosition = position;
        mPosition.X = (float)(Math.Round(mPosition.X));
        mPosition.Y = (float)(Math.Round(mPosition.Y));
        mViewTransformationMatrixChanged = true;
    }

    private void MoveCamera(InputState inputState)
    {
        if (inputState.mActionList.Contains(ActionType.CameraUp))
        {
            mPosition += new Vector2(0, -5);
        }
        if (inputState.mActionList.Contains(ActionType.CameraDown))
        {
            mPosition += new Vector2(0, 5);
        }
        if (inputState.mActionList.Contains(ActionType.CameraLeft))
        {
            mPosition += new Vector2(-5, 0);
        }
        if (inputState.mActionList.Contains(ActionType.CameraRight))
        {
            mPosition += new Vector2(5, 0);
        }
    }

    private void AdjustZoom(GameTime gameTime, InputState inputState)
    {
        // adjust zoom
        int zoom = 0;
        if (inputState.mActionList.Contains(ActionType.CameraZoomIn) && mZoom < mMimZoom)
        {
            zoom += 1;
        }
        if (inputState.mActionList.Contains(ActionType.CameraZoomOut) && mZoom > mMaxZoom)
        {
            zoom -= 1;
        }
        if (inputState.mActionList.Contains(ActionType.CameraZoomInFast) && mZoom < mMimZoom)
        {
            zoom += 4;
        }
        if (inputState.mActionList.Contains(ActionType.CameraZoomOutFast) && mZoom > mMaxZoom)
        {
            zoom -= 4;
        }
        if (zoom != 0)
        {
            Zoom(1 + (zoom * 0.001f * gameTime.ElapsedGameTime.Milliseconds));
        }
    }

    public void SetZoom(float zoom)
    {
        mZoom = zoom;
        mViewTransformationMatrixChanged = true;
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
        if(mViewTransformationMatrixChanged) {
            mTransform = Matrix.CreateTranslation(new Vector3(-mPosition.X, -mPosition.Y, 0)) 
                         * Matrix.CreateScale(mZoom, mZoom, 1)
                         * Matrix.CreateTranslation(new Vector3(mWidth / 2f, mHeight / 2f, 0));
            mViewTransformationMatrixChanged = false;
        }
        return mTransform;
    }

    public void Update(GameTime gameTime, InputState inputState)
    {
        AdjustZoom(gameTime, inputState);
        MoveCamera(inputState);
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

        SetPosition(mPosition);
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
            graphX[i] = (int)(random.NextDouble() * radius - radius/2f);
            graphY[i] = (int)(random.NextDouble() * radius - radius/2f);
        }

        mAnimationX = graphX;
        mAnimationY = graphY;
    }
}
