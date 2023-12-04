// UiSlider.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
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

        public override void Initialize(Rectangle root) => OnResolutionChanged(root);

        public override void Update(InputState inputState, Rectangle root)
        {

            if (inputState.Actions.Contains(ActionType.LeftClickReleased)) mWasPressed = false;
            if (mCanvas.Contains(inputState.mMousePosition) &&
                inputState.Actions.Contains(ActionType.LeftClick)) mWasPressed = true;

            if (!mWasPressed) return;
            mSliderValue = GetValue(inputState.mMousePosition);
            mSliderDotPosition = sliderPosition;
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
            var sliderPos = new Vector2(mCanvas.Bounds.Left, mCanvas.Center.Y);
            var dotDim = new Vector2(mCanvas.Bounds.Height, mCanvas.Bounds.Height);

            TextureManager.Instance.DrawLine(sliderPos, mSliderDotPosition, Color.White, 6, 1);
            TextureManager.Instance.DrawLine(sliderPos, mSliderLength, new(100, 100, 100, 100), 6, 1);
            TextureManager.Instance.Draw(TextureRegistries.sliderDot, mSliderDotPosition - (dotDim / 2), dotDim.X, dotDim.Y, mWasPressed ? Color.MonoGameOrange : Color.White);
            mCanvas.Draw();
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            Height = 25;
            mCanvas.UpdateFrame(root);
            mSliderLength = MathF.Abs(mCanvas.Bounds.Left - mCanvas.Bounds.Right);
            mSliderDotPosition = sliderPosition;
        }

        private Vector2 sliderPosition => new(mCanvas.Bounds.Left + mSliderLength * mSliderValue, mCanvas.Center.Y);
    }
}
