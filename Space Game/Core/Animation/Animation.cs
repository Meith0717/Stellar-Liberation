using System;
using Newtonsoft.Json;

namespace rache_der_reti.Core.Animation;
[Serializable]
public class Animation
{
    [JsonProperty] private int[] mFrames;
    [JsonProperty] private bool mCyclingOn;
    [JsonProperty] private int mFramesPerSecond;

    [JsonProperty] private int mTotalFrames;
    [JsonProperty] private int mCurrentFrameIndex;

    [JsonProperty] private float mAnimationFrameFloat;

    public Animation(int[] frames, int totalFrames, int framesPerSecond, bool cyclingOn)
    {
        mFrames = frames;
        mTotalFrames = totalFrames;
        mFramesPerSecond = framesPerSecond;
        mCyclingOn = cyclingOn;
    }

    public void Update(int milliseconds)
    {
        if ((!mCyclingOn && mCurrentFrameIndex < mFrames.Length - 1) || mCyclingOn)
        {
            mAnimationFrameFloat += milliseconds / 1000f * mFramesPerSecond;
            mCurrentFrameIndex = (int)(mAnimationFrameFloat) % mFrames.Length;
        }
    }

    public void SetFrame(int x)
    {
        mCurrentFrameIndex = x;
        mAnimationFrameFloat = x;
    }

    public void Reset()
    {
        mCurrentFrameIndex = 0;
        mAnimationFrameFloat = 0;
    }

    public int GetCurrentFrame()
    {
        return mFrames[mCurrentFrameIndex];
    }

    public int GetTotalFrames()
    {
        return mTotalFrames;
    }
}