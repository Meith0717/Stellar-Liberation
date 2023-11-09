// UiButton.cs 
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
        public Action OnClickAction;
        protected string mSpriteId;
        private string mText;
        private float mTextScale;
        private bool mIsDisabled;

        public UiButton(string SpriteId, string text, float textScale = 1)
        {
            mTextScale = textScale;
            mText = text;
            mSpriteId = SpriteId;
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            Width = texture.Width;
            Height = texture.Height;
        }

        public override void Draw() 
        {
            var color = IsHover ? Color.MonoGameOrange : Color.White;
            var position = new Vector2(Frame.Location.X + 5, Frame.Location.Y + 10);
            TextureManager.Instance.Draw(mSpriteId, Frame.Location.ToVector2(), Frame.Width, Frame.Height, color);
            TextureManager.Instance.DrawString("button", position, mText, mTextScale, color);
        } 

        public override void Update(InputState inputState, Rectangle root) 
        {
            mIsDisabled = OnClickAction is null;
            if (mIsDisabled) return;
            IsHover = Frame.Contains(inputState.mMousePosition);
            if (IsHover && OnClickAction is not null) inputState.DoMouseAction(MouseActionType.LeftClick, OnClickAction);
        }
    }
}
