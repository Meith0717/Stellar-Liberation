// UiSlider.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiSlider : UiElement
    {

        private float mSliderLength;
        private float mSliderValue;
        private readonly string mText;
        private Vector2 mTextDim;

        private bool mWasPressed;

        public float Value => mSliderValue;

        public UiSlider(string text, float val) 
        {            
            mSliderValue = val;
            mText = text;
            mCanvas.Height = 50;
            mTextDim = TextureManager.Instance.GetFont(FontRegistries.button).MeasureString(mText);
        }

        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            mSliderLength = MathF.Abs((mCanvas.Bounds.Left + mTextDim.X) - mCanvas.Bounds.Right);
            mSliderDotPosition = sliderPosition;
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            if (!inputState.Actions.Contains(ActionType.LeftClickHold))
            {
                mWasPressed = false;
                return;
            }

            if (!mCanvas.Contains(inputState.mMousePosition) && !mWasPressed) return;

            mWasPressed = true;

            mSliderValue = GetValue(inputState.mMousePosition);
            mSliderDotPosition = sliderPosition;
        }

        private float GetValue(Vector2 mousePos)
        {
            var relMouselength = mousePos.X - (mCanvas.Bounds.Left + mTextDim.X);
            var tmp = relMouselength / mSliderLength;
            if (tmp > 1) return 1;
            if (tmp < 0) return 0;
            return tmp;
        }

        private Vector2 mSliderDotPosition;

        public override void Draw()
        {
            var textPos = new Vector2(mCanvas.Bounds.Left, mCanvas.Center.Y - (mTextDim.Y / 2) + 3);
            var sliderPos = new Vector2(mCanvas.Bounds.Left + mTextDim.X, mCanvas.Center.Y);

            TextureManager.Instance.DrawString(FontRegistries.button, textPos, mText, 1, Color.White);
            TextureManager.Instance.DrawLine(sliderPos, mSliderLength, Color.White, 6, 1);
            TextureManager.Instance.Draw(TextureRegistries.sliderDot, mSliderDotPosition - new Vector2(10, 10), 20, 20);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            mSliderLength = MathF.Abs((mCanvas.Bounds.Left + mTextDim.X) - mCanvas.Bounds.Right);
            mSliderDotPosition = sliderPosition;
        }

        private Vector2 sliderPosition => new((mCanvas.Bounds.Left + mTextDim.X) + mSliderLength * mSliderValue, mCanvas.Center.Y);
    }
}
