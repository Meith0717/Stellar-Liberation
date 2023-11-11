// UiBare.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiBare : UiElement
    {
        private double mPercentage = 1;

        private RectangleF mMidShadowFrame;
        private RectangleF mLeftShadowFrame;
        private RectangleF mRightShadowFrame;

        private RectangleF mMidFrame;
        private RectangleF mLeftFrame;
        private RectangleF mRightFrame;

        private Color mColor;

        public UiBare(Color color) => mColor = color;

        public override void Update(InputState inputState, Rectangle root)
        {
            var ratio = 26 / 6f;

            var shadowFrameWidth = (int)(Frame.Height / ratio);
            var shadowFrameHeight = Frame.Height;

            mLeftShadowFrame = new(Frame.Location.X, Frame.Location.Y, shadowFrameWidth, shadowFrameHeight);
            mRightShadowFrame = new(Frame.Right - shadowFrameWidth, Frame.Location.Y, shadowFrameWidth, shadowFrameHeight);
            mMidShadowFrame = new(Frame.Location.X + shadowFrameWidth, Frame.Location.Y, Frame.Width - shadowFrameWidth * 2, Frame.Height);

            mLeftFrame = mLeftShadowFrame;
            mMidShadowFrame.Width *= (float)mPercentage;
            mMidFrame = mMidShadowFrame;
            mRightShadowFrame.Position = new(mMidFrame.Right, mMidFrame.Position.Y);
            mRightFrame = mRightShadowFrame;
        }

        public double Percentage { get { return mPercentage; } set  { if (value > 0 && value <= 1) mPercentage = value; }}

        public override void Draw()
        {
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalShadowLeft, mLeftShadowFrame.Position, mLeftShadowFrame.Width, mLeftShadowFrame.Height);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalShadowRight, mRightShadowFrame.Position, mRightShadowFrame.Width, mRightShadowFrame.Height);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalShadowMid, mMidShadowFrame.Position, mMidShadowFrame.Width, mMidShadowFrame.Height);

            TextureManager.Instance.Draw(TextureRegistries.barHorizontalLeft, mLeftFrame.Position, mLeftFrame.Width, mLeftFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalRight, mRightFrame.Position, mRightFrame.Width, mRightFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalMid, mMidFrame.Position, mMidFrame.Width, mMidFrame.Height, mColor);


        }
    }
}
