// UiButton.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using System;

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

        public UiButton(string SpriteId, string text, TextAllign textAllign = TextAllign.Center)
            : base(SpriteId, 1)
        {
            mText = new("neuropolitical", text, .1f)
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

        public override void Update(InputState inputState, GameTime gameTime)
        {
            IsHover = Canvas.Contains(inputState.mMousePosition);
            if (IsHover && !IsDisabled && IsHover != mLastHoverState) SoundEffectManager.Instance.PlaySound("hover");
            mLastHoverState = IsHover;
            if (IsHover)
                inputState.DoAction(ActionType.LeftReleased, () =>
                {
                    OnClickAction?.Invoke();
                    IsHover = false;
                });
            Color = IsDisabled ? Color.Transparent : IsHover ? Color.MonoGameOrange : new(128, 128, 128);
            mText.Update(inputState, gameTime);
            base.Update(inputState, gameTime);
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            base.ApplyResolution(root, resolution);
            mText.ApplyResolution(Bounds, resolution);
        }

        public override void Draw()
        {
            base.Draw();
            mText?.Draw();
        }
    }
}
