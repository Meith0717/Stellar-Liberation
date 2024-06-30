// UiSlider.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiSlider : UiElement
    {

        private float mSliderLength;
        private float mSliderValue;


        private bool mWasHovered;

        public float Value => mSliderValue;

        public UiSlider(float val) => mSliderValue = val;

        public override void Update(InputState inputState, GameTime gameTime)
        {
            mSliderLength = MathF.Abs(Bounds.Left - Bounds.Right);

            mSliderDotPosition = sliderPosition;
            if (!Contains(inputState.mMousePosition)) return;
            if (!inputState.HasAction(ActionType.LeftClickHold)) return;
            mSliderValue = GetValue(inputState.mMousePosition);
            mSliderDotPosition = sliderPosition;
            base.Update(inputState, gameTime);
        }

        private float GetValue(Vector2 mousePos)
        {
            var relMouselength = mousePos.X - Bounds.Left;
            var tmp = relMouselength / mSliderLength;
            if (tmp > 1) return 1;
            if (tmp < 0) return 0;
            return tmp;
        }

        private Vector2 mSliderDotPosition;

        public override void Draw()
        {
            var sliderPos = new Vector2(Bounds.Left, Bounds.Center.Y);
            var dotDim = new Vector2(Bounds.Height, Bounds.Height) * 1.2f;

            TextureManager.Instance.DrawLine(sliderPos, mSliderLength, Color.Black * 0.9f, 10 * mUiScale, 1);
            TextureManager.Instance.DrawLine(sliderPos, mSliderDotPosition, Color.MonoGameOrange, 10 * mUiScale, 0);
            TextureManager.Instance.Draw("dot", mSliderDotPosition - (dotDim / 2), dotDim.X, dotDim.Y, mWasHovered ? Color.DarkGray : Color.White);
            DrawCanvas();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            Height = 25;
            base.ApplyResolution(root, resolution);
        }

        private Vector2 sliderPosition => new(Bounds.Left + mSliderLength * mSliderValue, Center.Y);
    }
}
