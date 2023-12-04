﻿// UiDescriber.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
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

        public override void Initialize(Rectangle root)
        {
            OnResolutionChanged(root);
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            mUiElement.Update(inputState, root);
        }


        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            mUiElement.OnResolutionChanged(mCanvas.Bounds);
            mText.OnResolutionChanged(mCanvas.Bounds);
        }

        public override void Draw()
        {
            mUiElement.Draw();
            mText.Draw();
            mCanvas.Draw();
        }
    }
}