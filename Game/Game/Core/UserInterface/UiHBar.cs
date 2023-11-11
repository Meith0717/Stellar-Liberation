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
    internal class UiHBar : UiElement
    {
        private double mPercentage;

        private RectangleF mTextureFrame;

        private RectangleF mMidShadowFrame;
        private RectangleF mLeftShadowFrame;
        private RectangleF mRightShadowFrame;

        private RectangleF mMidFrame;
        private RectangleF mLeftFrame;
        private RectangleF mRightFrame;

        private string mTexture;

        private Color mColor;

        public UiHBar(Color color, string texture) 
        { 
            mTexture = texture; 
            mColor = color; 
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            var ratio = 26 / 6f;

            mTextureFrame = new(Frame.Location.X, Frame.Location.Y, Frame.Height, Frame.Height);

            var sideFrameWidth = (int)(Frame.Height / ratio);
            var sideFrameHeight = Frame.Height;

            mLeftShadowFrame = new(mTextureFrame.Right, mTextureFrame.Y, sideFrameWidth, sideFrameHeight);
            mMidShadowFrame = new(mLeftShadowFrame.Right, Frame.Y, Frame.Width - sideFrameWidth * 2 - mTextureFrame.Width, Frame.Height);
            mRightShadowFrame = new(mMidShadowFrame.Right, Frame.Y, sideFrameWidth, sideFrameHeight);

            mLeftFrame = mLeftShadowFrame;
            mMidFrame = new(mMidShadowFrame.X, mMidShadowFrame.Y, mMidShadowFrame.Width * (float)Percentage, mMidShadowFrame.Height); ;
            mRightFrame = new(mMidFrame.Right, mMidFrame.Y, sideFrameWidth, sideFrameHeight);
        }

        public double Percentage { get { return mPercentage; } set  { if (value > 0 && value <= 1) mPercentage = value; }}

        public override void Draw()
        {
            TextureManager.Instance.Draw(mTexture, mTextureFrame.Position, mTextureFrame.Width, mTextureFrame.Height, mColor);

            TextureManager.Instance.Draw(TextureRegistries.barHorizontalShadowLeft, mLeftShadowFrame.Position, mLeftShadowFrame.Width, mLeftShadowFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalShadowRight, mRightShadowFrame.Position, mRightShadowFrame.Width, mRightShadowFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalShadowMid, mMidShadowFrame.Position, mMidShadowFrame.Width, mMidShadowFrame.Height, mColor);

            TextureManager.Instance.Draw(TextureRegistries.barHorizontalLeft, mLeftFrame.Position, mLeftFrame.Width, mLeftFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalRight, mRightFrame.Position, mRightFrame.Width, mRightFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barHorizontalMid, mMidFrame.Position, mMidFrame.Width, mMidFrame.Height, mColor);
        }
    }
}
