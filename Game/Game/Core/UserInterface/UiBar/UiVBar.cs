﻿// UiVBar.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;

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

        public override void Update(InputState inputState, Rectangle root)
        {
            var ratio = 26 / 6f;

            mTextureFrame = new(mCanvas.Position.X, mCanvas.Position.Y, mCanvas.Bounds.Width, mCanvas.Bounds.Width);

            var sideFrameWidth = mCanvas.Bounds.Width;
            var sideFrameHeight = (int)(mCanvas.Bounds.Width / ratio);

            mTopShadowFrame = new(mCanvas.Position.X, mTextureFrame.Bottom, sideFrameWidth, sideFrameHeight);
            mMidShadowFrame = new(mCanvas.Position.X, mTopShadowFrame.Bottom, mCanvas.Bounds.Width, mCanvas.Bounds.Height - sideFrameHeight * 2 - mTextureFrame.Height);
            mBottomShadowFrame = new(mCanvas.Position.X, mMidShadowFrame.Bottom, sideFrameWidth, sideFrameHeight);

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

        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
        }
    }
}