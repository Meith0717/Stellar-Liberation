﻿// UiHBar.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface.UiBar
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

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            var ratio = 26 / 6f;

            mTextureFrame = new(Canvas.Position.X, Canvas.Position.Y, Canvas.Bounds.Height, Canvas.Bounds.Height);

            var sideFrameWidth = (int)(Canvas.Bounds.Height / ratio);
            var sideFrameHeight = Canvas.Bounds.Height;

            mLeftShadowFrame = new(mTextureFrame.Right, mTextureFrame.Y, sideFrameWidth, sideFrameHeight);
            mMidShadowFrame = new(mLeftShadowFrame.Right, Canvas.Position.Y, Canvas.Bounds.Width - sideFrameWidth * 2 - mTextureFrame.Width, Canvas.Bounds.Height);
            mRightShadowFrame = new(mMidShadowFrame.Right, Canvas.Position.Y, sideFrameWidth, sideFrameHeight);

            mLeftFrame = mLeftShadowFrame;
            mMidFrame = new(mMidShadowFrame.X, mMidShadowFrame.Y, mMidShadowFrame.Width * (float)Percentage, mMidShadowFrame.Height); ;
            mRightFrame = new(mMidFrame.Right, mMidFrame.Y, sideFrameWidth, sideFrameHeight);
        }

        public double Percentage { get { return mPercentage; } set { if (value > 0 && value <= 1) mPercentage = value; } }

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
