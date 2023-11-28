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

        private bool mWasPressed;

        public float Value => mSliderValue;

        public UiSlider(float val) 
        {            
            mSliderValue = val;
        }

        public UiSlider(float val, int width, int x, int y)
        {
            mSliderValue = val;
            mCanvas.Width = width; mCanvas.Height = 50;
            mCanvas.X = x; mCanvas.Y = y;
        }

        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            mSliderLength = MathF.Abs(mCanvas.Bounds.Left - mCanvas.Bounds.Right);
            mSliderDotPosition = new(mCanvas.Bounds.Left + mSliderLength * mSliderValue, mCanvas.Center.Y);
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            if (!inputState.HasAction(ActionType.LeftClickHold))
            {
                mWasPressed = false;
                return;
            }

            if (!mCanvas.Contains(inputState.mMousePosition) && !mWasPressed) return;

            mWasPressed = true;

            mSliderValue = GetValue(inputState.mMousePosition);
            mSliderDotPosition = new(mCanvas.Bounds.Left + mSliderLength * mSliderValue, mCanvas.Center.Y);
        }

        private float GetValue(Vector2 mousePos)
        {
            var relMouselength = mousePos.X - mCanvas.Bounds.Left;
            var tmp = relMouselength / mSliderLength;
            if (tmp > 1) return 1;
            if (tmp < 0) return 0;
            return tmp;
        }

        private Vector2 mSliderDotPosition;

        public override void Draw()
        {
            var start = new Vector2(mCanvas.Bounds.Left, mCanvas.Center.Y);
            TextureManager.Instance.DrawLine(start, mSliderLength, Color.White, 5, 1);
            TextureManager.Instance.Draw(TextureRegistries.sliderDot, mSliderDotPosition - new Vector2(10, 10), 20, 20);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            mSliderLength = MathF.Abs(mCanvas.Bounds.Left - mCanvas.Bounds.Right);
            mSliderDotPosition = new(mCanvas.Bounds.Left + mSliderLength * mSliderValue, mCanvas.Center.Y);
        }
    }
}
