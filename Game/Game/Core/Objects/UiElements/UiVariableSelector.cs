// UiVariableSelector.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
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
            mLeftArrow = new(TextureRegistries.arrowL, "") { Anchor = Anchor.W, FillScale = FillScale.Y, OnClickAction = () => { DecrementIndex(); OnClickAction(); } };
            mRightArrow = new(TextureRegistries.arrowR, "") { Anchor = Anchor.E, FillScale = FillScale.Y, OnClickAction = () => { IncrementIndex(); OnClickAction(); } };
            mVariable = new(FontRegistries.buttonFont, "N.A") { Anchor = Anchor.Center };
        }

        public void Add(T variable) => mVariables.Add(variable);
        public void AddRange(List<T> variables) => mVariables.AddRange(variables);

        private void IncrementIndex() => mIndex += (mIndex < mVariables.Count - 1) ? 1 : 0;

        private void DecrementIndex() => mIndex -= (mIndex > 0) ? 1 : 0;

        public T Value => mVariables[mIndex];

        public override void Initialize(Rectangle root, float UiScaling)
        {
            OnResolutionChanged(root, 1);
        }

        public override void Update(InputState inputState, Rectangle root, float UiScaling)
        {
            mVariable.Update(inputState, Canvas.Bounds, 1);
            mLeftArrow.Update(inputState, Canvas.Bounds, 1);
            mRightArrow.Update(inputState, Canvas.Bounds, 1);
            mVariable.Text = mVariables[mIndex].ToString();
            mLeftArrow.IsDisabled = mIndex == 0;
            mRightArrow.IsDisabled = mIndex == mVariables.Count - 1;
        }

        public override void Draw()
        {
            mLeftArrow.Draw();
            mRightArrow.Draw();
            mVariable.Draw();
            Canvas.Draw();
        }

        public override void OnResolutionChanged(Rectangle root, float UiScaling)
        {
            Canvas.Height = (int)TextureManager.Instance.GetFont(FontRegistries.buttonFont).MeasureString(" ").Y;
            Canvas.UpdateFrame(root, UiScaling);
            mLeftArrow.Initialize(Canvas.Bounds, UiScaling);
            mRightArrow.Initialize(Canvas.Bounds, UiScaling);
            mVariable.Initialize(Canvas.Bounds, UiScaling);
        }
    }
}
