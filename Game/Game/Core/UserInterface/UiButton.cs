﻿// UiButton.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.InputManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiButton : UiElement
    {
        public bool IsHover;
        public bool IsDisabled;
        public Action OnClickAction;
        protected string mSpriteId;

        public UiButton(string SpriteId)
        {
            mSpriteId = SpriteId;
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = texture.Width;
            Height = texture.Height;
        }

        public override void Draw() => TextureManager.Instance.Draw(mSpriteId, Frame.Location.ToVector2(), Frame.Width, Frame.Height, IsDisabled ? new Color(52, 73, 94) : IsHover ? Color.MonoGameOrange : Color.White);

        public override void Update(InputState inputState, Rectangle root) 
        {
            if (IsDisabled) return;
            IsHover = Frame.Contains(inputState.mMousePosition);
            if (IsHover && OnClickAction is not null) inputState.DoMouseAction(MouseActionType.LeftClick, OnClickAction);
        }

    }
}
