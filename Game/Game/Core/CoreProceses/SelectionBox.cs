// SelectionBox.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace StellarLiberation.Game.Core.CoreProceses
{
    internal class SelectionBox
    {
        private Vector2 mStartPosition;
        private RectangleF mBox = new();

        public SelectionBox(Vector2 position)
            => mStartPosition = position;

        public void Update(Vector2 position)
        {
            mBox.Width = Math.Abs(position.X - mStartPosition.X);
            mBox.Height = Math.Abs(position.Y - mStartPosition.Y);

            if (mStartPosition.X < position.X)
            {
                mBox.X = mStartPosition.X;
                mBox.Y = (mStartPosition.Y < position.Y) ? mStartPosition.Y : position.Y;
                return;
            }

            mBox.X = position.X;
            mBox.Y = (mStartPosition.Y < position.Y) ? mStartPosition.Y : position.Y;

        }

        public RectangleF ToRectangleF() => mBox;

        public float Length => new Vector2(mBox.Size.Width, mBox.Size.Height).Length();
    }
}
