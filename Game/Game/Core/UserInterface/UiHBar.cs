// UiHBar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

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

        public override void Update(InputState inputState, GameTime gameTime)
        {
            var ratio = 2f;
            var sideFrameWidth = (int)(Bounds.Height / ratio);
            var sideFrameHeight = Bounds.Height;
            if (mTexture == "")
            {
                mLeftShadowFrame = new(Bounds.X, Bounds.Y, sideFrameWidth, sideFrameHeight);
                mMidShadowFrame = new(mLeftShadowFrame.Right, Position.Y, Bounds.Width - sideFrameWidth * 2, Bounds.Height);
                mRightShadowFrame = new(mMidShadowFrame.Right, Position.Y, sideFrameWidth, sideFrameHeight);

                mLeftFrame = mLeftShadowFrame;
                mMidFrame = new(mMidShadowFrame.X, mMidShadowFrame.Y, mMidShadowFrame.Width * (float)Percentage, mMidShadowFrame.Height); ;
                mRightFrame = new(mMidFrame.Right, mMidFrame.Y, sideFrameWidth, sideFrameHeight);

                return;
            }

            mTextureFrame = new(Position.X, Position.Y, Bounds.Height, Bounds.Height);

            mLeftShadowFrame = new(mTextureFrame.Right, mTextureFrame.Y, sideFrameWidth, sideFrameHeight);
            mMidShadowFrame = new(mLeftShadowFrame.Right, Position.Y, Bounds.Width - sideFrameWidth * 2 - mTextureFrame.Width, Bounds.Height);
            mRightShadowFrame = new(mMidShadowFrame.Right, Position.Y, sideFrameWidth, sideFrameHeight);

            mLeftFrame = mLeftShadowFrame;
            mMidFrame = new(mMidShadowFrame.X, mMidShadowFrame.Y, mMidShadowFrame.Width * (float)Percentage, mMidShadowFrame.Height); ;
            mRightFrame = new(mMidFrame.Right, mMidFrame.Y, sideFrameWidth, sideFrameHeight);
        }

        public double Percentage { get { return (float)mPercentage; } set {  mPercentage = MathHelper.Clamp((float)value, 0, 1); } }

        public override void Draw()
        {
            if (mTexture != "")
                TextureManager.Instance.Draw(mTexture, mTextureFrame.Position, mTextureFrame.Width, mTextureFrame.Height, mColor);

            TextureManager.Instance.Draw("barHorizontalShadowLeft", mLeftShadowFrame.Position, mLeftShadowFrame.Width, mLeftShadowFrame.Height, new(10, 10, 10));
            TextureManager.Instance.Draw("barHorizontalShadowRight", mRightShadowFrame.Position, mRightShadowFrame.Width, mRightShadowFrame.Height, new(10, 10, 10));
            TextureManager.Instance.Draw("barHorizontalShadowMid", mMidShadowFrame.Position, mMidShadowFrame.Width, mMidShadowFrame.Height, new(10, 10, 10));

            TextureManager.Instance.Draw("barHorizontalLeft", mLeftFrame.Position, mLeftFrame.Width, mLeftFrame.Height, mColor);
            TextureManager.Instance.Draw("barHorizontalRight", mRightFrame.Position, mRightFrame.Width, mRightFrame.Height, mColor);
            TextureManager.Instance.Draw("barHorizontalMid", mMidFrame.Position, mMidFrame.Width, mMidFrame.Height, mColor);
            DrawCanvas();
        }

    }
}
