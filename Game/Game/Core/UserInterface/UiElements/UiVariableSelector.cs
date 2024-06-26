// UiVariableSelector.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiVariableSelector<T> : UiElement
    {
        private readonly List<T> mVariables;
        private readonly UiButton mLeftArrow;
        private readonly UiButton mRightArrow;
        private readonly UiText mVariable;
        private int mIndex;
        public Action OnClickAction;

        public UiVariableSelector(List<T> variables, T initalValue)
        {
            mVariables = variables;
            mIndex = variables.IndexOf(initalValue);
            if (mIndex < 0) mIndex = 0;
            mLeftArrow = new("arrowL", "", TextAllign.W) { Anchor = Anchor.W, OnClickAction = () => { DecrementIndex(); OnClickAction?.Invoke(); } };
            mRightArrow = new("arrowR", "", TextAllign.W) { Anchor = Anchor.E, OnClickAction = () => { IncrementIndex(); OnClickAction?.Invoke(); } };
            mVariable = new("neuropolitical", initalValue.ToString(), .1f) { Anchor = Anchor.Center };
        }

        public void Add(T variable) => mVariables.Add(variable);
        public void AddRange(List<T> variables) => mVariables.AddRange(variables);
        private void IncrementIndex() => mIndex += (mIndex < mVariables.Count - 1) ? 1 : 0;
        private void DecrementIndex() => mIndex -= (mIndex > 0) ? 1 : 0;
        public T Value => mVariables[mIndex];

        public override void Update(InputState inputState, GameTime gameTime)
        {
            mVariable.Text = mVariables[mIndex].ToString();
            mVariable.Update(inputState, gameTime);
            mLeftArrow.Update(inputState, gameTime);
            mLeftArrow.IsDisabled = mIndex == 0;
            mRightArrow.Update(inputState, gameTime);
            mRightArrow.IsDisabled = mIndex == (mVariables.Count - 1);
        }

        public override void Draw()
        {
            mLeftArrow.Draw();
            mRightArrow.Draw();
            mVariable.Draw();
            Canvas.Draw();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            base.ApplyResolution(root, resolution);
            Canvas.UpdateFrame(root, resolution.UiScaling);
            mVariable.ApplyResolution(Bounds, resolution);

            mLeftArrow.Width = mVariable.Bounds.Width;
            mLeftArrow.Height = mVariable.Bounds.Height;
            mLeftArrow.ApplyResolution(Bounds, resolution);
            mLeftArrow.IsDisabled = mIndex == 0;

            mRightArrow.Width = mVariable.Bounds.Width;
            mRightArrow.Height = mVariable.Bounds.Height;
            mRightArrow.ApplyResolution(Bounds, resolution);
        }
    }
}
