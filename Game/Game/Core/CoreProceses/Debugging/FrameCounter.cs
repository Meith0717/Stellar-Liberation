// FrameCounter.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.Debugging
{
    public class FrameCounter
    {
        private const int MaximumSamples = 1000;
        private readonly Queue<float> mSampleBuffer = new();
        private GameTime mGameTime;
        private double mStartTime, mEndTime;
        private readonly int mMaxCoolDown;
        private int mCoolDown;

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float MinFramesPerSecond { get; private set; } = float.MaxValue;
        public float MaxFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }
        public float FrameDuration { get; private set; }

        public FrameCounter(int cooldown = 0) => mMaxCoolDown = cooldown;

        public void Update(GameTime gameTime) 
        {
            mGameTime = gameTime;
            if (mCoolDown <= 0) mCoolDown = mMaxCoolDown;
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
        } 

        public void UpdateFrameCouning()
        {
            if (mGameTime == null) { return; }
            if (mCoolDown > 0) { return; }
            mStartTime = mGameTime.ElapsedGameTime.TotalMilliseconds;
            float deltaTime = (float)(mGameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            FrameDuration = mGameTime.ElapsedGameTime.Milliseconds;
            CurrentFramesPerSecond = 1.0f / deltaTime;
            MinFramesPerSecond = MathHelper.Min(CurrentFramesPerSecond, MinFramesPerSecond);
            MaxFramesPerSecond = MathHelper.Max(CurrentFramesPerSecond, MaxFramesPerSecond);
            mSampleBuffer.Enqueue(CurrentFramesPerSecond);
            if (mSampleBuffer.Count > MaximumSamples) mSampleBuffer.Dequeue();
            AverageFramesPerSecond = mSampleBuffer.Average(i => i);
            TotalSeconds += (float)mGameTime.ElapsedGameTime.TotalSeconds;
            TotalFrames++;
        }
    }
}
