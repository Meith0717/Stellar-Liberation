// UiVBar.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiBar
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

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            var ratio = 26 / 6f;

            mTextureFrame = new(Canvas.Position.X, Canvas.Position.Y, Canvas.Bounds.Width, Canvas.Bounds.Width);

            var sideFrameWidth = Canvas.Bounds.Width;
            var sideFrameHeight = (int)(Canvas.Bounds.Width / ratio);

            mTopShadowFrame = new(Canvas.Position.X, mTextureFrame.Bottom, sideFrameWidth, sideFrameHeight);
            mMidShadowFrame = new(Canvas.Position.X, mTopShadowFrame.Bottom, Canvas.Bounds.Width, Canvas.Bounds.Height - sideFrameHeight * 2 - mTextureFrame.Height);
            mBottomShadowFrame = new(Canvas.Position.X, mMidShadowFrame.Bottom, sideFrameWidth, sideFrameHeight);

            mBottomFrame = mBottomShadowFrame;
            mMidFrame = new(mMidShadowFrame.X, mBottomFrame.Top + 1f - mMidShadowFrame.Height * (float)Percentage, mMidShadowFrame.Width, mMidShadowFrame.Height * (float)Percentage);
            mTopFrame = new(mMidFrame.X, mMidFrame.Top - sideFrameHeight, sideFrameWidth, sideFrameHeight); ;
        }

        public double Percentage { get { return mPercentage; } set { if (value > 0 && value <= 1) mPercentage = value; } }

        public override void Draw()
        {
            TextureManager.Instance.Draw(mTexture, mTextureFrame.Position, mTextureFrame.Width, mTextureFrame.Height, mColor);

            TextureManager.Instance.Draw(TextureRegistries.barVerticalShadowTop, mTopShadowFrame.Position, mTopShadowFrame.Width, mTopShadowFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barVerticalShadowBottom, mBottomShadowFrame.Position, mBottomShadowFrame.Width, mBottomShadowFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barVerticalShadowMid, mMidShadowFrame.Position, mMidShadowFrame.Width, mMidShadowFrame.Height, mColor);

            TextureManager.Instance.Draw(TextureRegistries.barVerticalTop, mTopFrame.Position, mTopFrame.Width, mTopFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barVerticalBottom, mBottomFrame.Position, mBottomFrame.Width, mBottomFrame.Height, mColor);
            TextureManager.Instance.Draw(TextureRegistries.barVerticalMid, mMidFrame.Position, mMidFrame.Width, mMidFrame.Height, mColor);
        }

        public override void Initialize(Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
        }

        public override void OnResolutionChanged(Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
        }
    }
}
