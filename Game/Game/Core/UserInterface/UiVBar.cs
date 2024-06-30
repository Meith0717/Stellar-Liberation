// UiVBar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiVBar : UiElement
    {
        private double mPercentage;

        private RectangleF mTextureFrame;

        private RectangleF mMidShadowFrame;
        private RectangleF mTopShadowFrame;
        private RectangleF mBottomShadowFrame;

        private RectangleF mMidFrame;
        private RectangleF mTopFrame;
        private RectangleF mBottomFrame;

        private string mTexture;

        private Color mColor;

        public UiVBar(Color color, string texture)
        {
            mTexture = texture;
            mColor = color;
        }

        public override void Update(InputState inputState, GameTime gameTime)
        {
            var ratio = 26 / 6f;

            mTextureFrame = new(Position.X, Position.Y, Bounds.Width, Bounds.Width);

            var sideFrameWidth = Bounds.Width;
            var sideFrameHeight = (int)(Bounds.Width / ratio);

            mTopShadowFrame = new(Position.X, mTextureFrame.Bottom, sideFrameWidth, sideFrameHeight);
            mMidShadowFrame = new(Position.X, mTopShadowFrame.Bottom, Bounds.Width, Bounds.Height - sideFrameHeight * 2 - mTextureFrame.Height);
            mBottomShadowFrame = new(Position.X, mMidShadowFrame.Bottom, sideFrameWidth, sideFrameHeight);

            mBottomFrame = mBottomShadowFrame;
            mMidFrame = new(mMidShadowFrame.X, mBottomFrame.Top + 1f - mMidShadowFrame.Height * (float)Percentage, mMidShadowFrame.Width, mMidShadowFrame.Height * (float)Percentage);
            mTopFrame = new(mMidFrame.X, mMidFrame.Top - sideFrameHeight, sideFrameWidth, sideFrameHeight); ;
        }

        public double Percentage { get { return mPercentage; } set { if (value > 0 && value <= 1) mPercentage = value; } }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mTexture, mTextureFrame.Position, mTextureFrame.Width, mTextureFrame.Height, mColor);

            TextureManager.Instance.Draw("barVerticalShadowTop", mTopShadowFrame.Position, mTopShadowFrame.Width, mTopShadowFrame.Height, mColor);
            TextureManager.Instance.Draw("barVerticalShadowBottom", mBottomShadowFrame.Position, mBottomShadowFrame.Width, mBottomShadowFrame.Height, mColor);
            TextureManager.Instance.Draw("barVerticalShadowMid", mMidShadowFrame.Position, mMidShadowFrame.Width, mMidShadowFrame.Height, mColor);

            TextureManager.Instance.Draw("barVerticalTop", mTopFrame.Position, mTopFrame.Width, mTopFrame.Height, mColor);
            TextureManager.Instance.Draw("barVerticalBottom", mBottomFrame.Position, mBottomFrame.Width, mBottomFrame.Height, mColor);
            TextureManager.Instance.Draw("barVerticalMid", mMidFrame.Position, mMidFrame.Width, mMidFrame.Height, mColor);
            DrawCanvas();
        }
    }
}
