// UiButton.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    public enum TextAllign { W, E, Center }

    public class UiButton : UiElement
    {
        public bool IsDisabled {get; private set; }
        public bool IsHover;

        public Action OnClickAction;
        protected string mSpriteId;
        private string mText;
        private float mTextScale;
        public TextAllign TextAllign = TextAllign.W;
        private Vector2 TextPosition;

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
            TextureManager.Instance.Draw(mSpriteId, Frame.Location.ToVector2(), Frame.Width, Frame.Height, color);
            TextureManager.Instance.DrawString("button", TextPosition, mText, mTextScale, color);
        } 

        public override void Update(InputState inputState, Rectangle root) 
        {
            IsDisabled = OnClickAction is null;
            var stringDim = TextureManager.Instance.GetFont("button").MeasureString(mText);
            switch (TextAllign)
            {
                case TextAllign.W:
                    TextPosition = new Vector2(Frame.X + 5, Frame.Location.Y + 10);
                    break;
                case TextAllign.E:
                    TextPosition = new Vector2(Frame.Right - stringDim.X - 5, Frame.Location.Y + 10);
                    break;
                case TextAllign.Center:
                    TextPosition = new Vector2(Frame.Center.X - (stringDim.X / 2), Frame.Location.Y + 10);
                    break;
            }
            if (IsHover) inputState.DoAction(ActionType.Select, OnClickAction);
        }
    }
}
