﻿// UiButton.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System;
using System.Runtime.CompilerServices;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{
    public enum TextAllign { W, E, Center }

    public class UiButton : UiSprite
    {
        private readonly UiText mText;
        public bool IsDisabled;
        public bool IsHover;
        private bool mLastHoverState;
        public Action OnClickAction;
        private float mScaling;

        public UiButton(string SpriteId, string text, TextAllign textAllign = TextAllign.Center) 
            : base(SpriteId, 1)
        {
            mText = new(FontRegistries.buttonFont, text)
            {
                Anchor = textAllign switch
                {
                    TextAllign.W => Anchor.W,
                    TextAllign.E => Anchor.E,
                    TextAllign.Center => Anchor.Center,
                    _ => throw new System.NotImplementedException()
                }
            };
        }

        public override void Update(InputState inputState, GameTime gameTime, Rectangle root, float uiScaling)
        {
            IsHover = Canvas.Contains(inputState.mMousePosition);
            if (IsHover && !IsDisabled && IsHover != mLastHoverState) SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.hover);
            mLastHoverState = IsHover;
            if (IsHover & !IsDisabled)
            {
                mScaling += (float)(0.01d * gameTime.ElapsedGameTime.TotalMilliseconds);
                inputState.DoAction(ActionType.Select, () => { OnClickAction?.Invoke(); IsHover = false; mScaling = 1; });
            } else
            {
                mScaling -= (float)(0.01d * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            mScaling = MathHelper.Clamp(mScaling, 1, 1.05f);
            Color = IsDisabled ? new(20, 20, 20, 20) : IsHover ? new(51, 204, 204) : Color.White;

            base.Update(inputState, gameTime, root, uiScaling * mScaling);
            mText.Color = Color;
            mText?.Update(inputState, gameTime, Bounds, uiScaling * mScaling);
        }

        public override void Draw()
        {
            base.Draw();
            mText?.Draw();
        }
    }
}
