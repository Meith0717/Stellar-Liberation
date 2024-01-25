// UiSlider.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
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

        public UiSlider(float val) => mSliderValue = val;

        public UiSlider(ref float val) => mSliderValue = val;

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            Height = 25;
            Canvas.UpdateFrame(root, uiScaling);
            mSliderLength = MathF.Abs(Canvas.Bounds.Left - Canvas.Bounds.Right);

            mSliderDotPosition = sliderPosition;
            if (inputState.Actions.Contains(ActionType.LeftClickReleased)) mWasPressed = false;
            if (Canvas.Contains(inputState.mMousePosition) &&
                inputState.Actions.Contains(ActionType.LeftClick)) mWasPressed = true;

            if (!mWasPressed) return;
            mSliderValue = GetValue(inputState.mMousePosition);
            mSliderDotPosition = sliderPosition;
        }

        private float GetValue(Vector2 mousePos)
        {
            var relMouselength = mousePos.X - Canvas.Bounds.Left;
            var tmp = relMouselength / mSliderLength;
            if (tmp > 1) return 1;
            if (tmp < 0) return 0;
            return tmp;
        }

        private Vector2 mSliderDotPosition;

        public override void Draw()
        {
            var sliderPos = new Vector2(Canvas.Bounds.Left, Canvas.Center.Y);
            var dotDim = new Vector2(Canvas.Bounds.Height, Canvas.Bounds.Height);

            TextureManager.Instance.DrawLine(sliderPos, mSliderDotPosition, Color.White, 6, 1);
            TextureManager.Instance.DrawLine(sliderPos, mSliderLength, new(100, 100, 100, 100), 6, 1);
            TextureManager.Instance.Draw(MenueSpriteRegistries.dot, mSliderDotPosition - (dotDim / 2), dotDim.X, dotDim.Y, mWasPressed ? new(192, 57, 43) : Color.White);
            Canvas.Draw();
        }


        private Vector2 sliderPosition => new(Canvas.Bounds.Left + mSliderLength * mSliderValue, Canvas.Center.Y);
    }
}
