﻿// UiVariableSelector.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiVariableSelector : UiElement
    {
        private readonly List<String> mVariables;
        private readonly UiButton mLeftArrow;
        private readonly UiButton mRightArrow;
        private readonly UiText mVariable;
        private int mIndex;

        public UiVariableSelector(List<String> variables)
        { 
            mVariables = variables;
            mLeftArrow = new(TextureRegistries.arrowL, "") { Anchor = Anchor.W, FillScale = FillScale.Y, OnClickAction = DecrementIndex };
            mRightArrow = new(TextureRegistries.arrowR, "") { Anchor = Anchor.E, FillScale = FillScale.Y, OnClickAction = IncrementIndex };
            mVariable = new(FontRegistries.buttonFont, "N.A") { Anchor = Anchor.Center };
        }

        private void IncrementIndex() => mIndex += (mIndex < mVariables.Count - 1) ? 1 : 0;

        private void DecrementIndex() => mIndex -= (mIndex > 0) ? 1 : 0;


        public override void Initialize(Rectangle root)
        {
            OnResolutionChanged(root);
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            mVariable.Update(inputState, mCanvas.Bounds);
            mLeftArrow.Update(inputState, mCanvas.Bounds);
            mRightArrow.Update(inputState, mCanvas.Bounds);
            mVariable.Text = mVariables[mIndex];
            mLeftArrow.IsDisabled = mIndex == 0;
            mRightArrow.IsDisabled = mIndex == mVariables.Count - 1;
        }

        public override void Draw()
        {
            mLeftArrow.Draw();
            mRightArrow.Draw();
            mVariable.Draw();
            mCanvas.Draw();
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.Height = (int)TextureManager.Instance.GetFont(FontRegistries.buttonFont).MeasureString(" ").Y;
            mCanvas.UpdateFrame(root);
            mLeftArrow.Initialize(mCanvas.Bounds);
            mRightArrow.Initialize(mCanvas.Bounds);
            mVariable.Initialize(mCanvas.Bounds);
        }

    }
}
