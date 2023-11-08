// Animation.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.Animations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace rache_der_reti.Core.Animation
{
    public class Animation
    {
        private List<Point> mFrames;
        private int mFrameIndex;
        private float mFrameFloat;
        private int mFramesPerSecond;
        private Vector2 mPosition;
        private bool mLoop;

        public bool IsRunning { get; private set; }

        public Animation(int framesPerSecond, List<Point> frames, bool loop)
        {
            mFramesPerSecond = framesPerSecond;
            mLoop = loop;
            mFrames = frames;
        }

        public void Update(Vector2 position, GameTime gameTime)
        {
            mPosition = position;
            IsRunning = false;
            if ((!mLoop && mFrameIndex < mFrames.Count - 1) || mLoop)
            {
                IsRunning = true;
                mFrameFloat += gameTime.ElapsedGameTime.Milliseconds / 1000f * mFramesPerSecond;
                mFrameIndex = (int)(mFrameFloat) % mFrames.Count;
                return;
            }
            Reset();
        }

        public void Reset() => mFrameFloat = mFrameIndex = 0;

        public void Draw(SpriteSheet spriteSheet, int depth) => TextureManager.Instance.DrawSpriteSheetFrame(spriteSheet, mPosition, mFrames[mFrameIndex], depth);

        public static List<Point> GetRowList(int row, int colLength)
        {
            var lst = new List<Point>();
            for (int i = 0; i < colLength - 1; i++)
            {
                lst.Add(new(i, row));
            }
            return lst;
        }
    }
}