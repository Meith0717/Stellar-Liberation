// UiVariableSelector.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
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
        public Action OnClickAction = () => throw new NotImplementedException();

        public UiVariableSelector(List<T> variables)
        {
            mVariables = variables;
            mLeftArrow = new(MenueSpriteRegistries.arrowL, "") { Anchor = Anchor.W, OnClickAction = () => { DecrementIndex(); OnClickAction(); } };
            mRightArrow = new(MenueSpriteRegistries.arrowR, "") { Anchor = Anchor.E, OnClickAction = () => { IncrementIndex(); OnClickAction(); } };
            mVariable = new(FontRegistries.buttonFont, "N.A") { Anchor = Anchor.Center };
        }

        public void Add(T variable) => mVariables.Add(variable);
        public void AddRange(List<T> variables) => mVariables.AddRange(variables);
        private void IncrementIndex() => mIndex += (mIndex < mVariables.Count - 1) ? 1 : 0;
        private void DecrementIndex() => mIndex -= (mIndex > 0) ? 1 : 0;
        public T Value => mVariables[mIndex];

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            mVariable.Text = mVariables[mIndex].ToString();
            mVariable.Update(inputState, Canvas.Bounds, uiScaling);

            mLeftArrow.Width = mVariable.Bounds.Width;
            mLeftArrow.Height = mVariable.Bounds.Height;
            mLeftArrow.Update(inputState, Canvas.Bounds, uiScaling);
            mLeftArrow.IsDisabled = mIndex == 0;

            mRightArrow.Width = mVariable.Bounds.Width;
            mRightArrow.Height = mVariable.Bounds.Height;
            mRightArrow.Update(inputState, Canvas.Bounds, uiScaling);
            mRightArrow.IsDisabled = mIndex == (mVariables.Count - 1);
        }

        public override void Draw()
        {
            mLeftArrow.Draw();
            mRightArrow.Draw();
            mVariable.Draw();
            Canvas.Draw();
        }
    }
}
