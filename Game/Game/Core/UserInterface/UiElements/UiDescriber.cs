﻿// UiDescriber.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiDescriber : UiElement
    {

        private readonly UiElement mUiElement;
        private readonly UiText mText;

        public UiDescriber(string text, UiElement uiElement)
        {
            mUiElement = uiElement;
            mUiElement.RelWidth = .6f;
            mUiElement.Anchor = Anchor.E;
            mText = new(FontRegistries.buttonFont, text) { Anchor = Anchor.W };
        }

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            mUiElement.Height = Canvas.Bounds.Height;
            mUiElement.Update(inputState, Canvas.Bounds, uiScaling);
            mText.Update(inputState, Canvas.Bounds, uiScaling);
        }

        public override void Draw()
        {
            mUiElement.Draw();
            mText.Draw();
            Canvas.Draw();
        }
    }
}