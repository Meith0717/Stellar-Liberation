// UiButton.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    public enum TextAllign { W, E, Center }

    public class UiButton : UiElement
    {
        public bool IsDisabled;
        public bool IsHover;
        private bool mLastHoverState;

        public Action OnClickAction;
        protected string mSpriteId;
        private string mText;
        private float mTextScale;
        public TextAllign TextAllign = TextAllign.W;
        private Vector2 TextPosition;

        public UiButton(string SpriteId, string text)
        {
            mText = text;
            mSpriteId = SpriteId;
        }

        public override void Draw()
        {
            var color = IsDisabled ? Color.DarkGray : IsHover ? new(51, 204, 204) : new(128, 128, 128);
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, color);
            TextureManager.Instance.DrawString(FontRegistries.buttonFont, TextPosition, mText, mTextScale, color);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            mTextScale = uiScaling;
            Width = texture.Width;
            Height = texture.Height;
            Canvas.UpdateFrame(root, uiScaling);

            var stringDim = TextureManager.Instance.GetFont(FontRegistries.buttonFont).MeasureString(mText);
            IsHover = Canvas.Contains(inputState.mMousePosition);
            if (IsHover && !IsDisabled && IsHover != mLastHoverState) SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.hover);
            mLastHoverState = IsHover;
            if (IsHover)
            {
                inputState.DoAction(ActionType.Select, Click);
            }
            var textHeight = Canvas.Center.Y - (stringDim.Y * uiScaling / 2 * mTextScale);
            switch (TextAllign)
            {
                case TextAllign.W:
                    TextPosition = new Vector2(Canvas.Bounds.Left, textHeight);
                    break;
                case TextAllign.E:
                    TextPosition = new Vector2(Canvas.Bounds.Right - (stringDim.X * uiScaling), textHeight);
                    break;
                case TextAllign.Center:
                    TextPosition = new Vector2(Canvas.Center.X - (stringDim.X * uiScaling / 2), textHeight);
                    break;
            }

        }

        private void Click()
        {
            if (OnClickAction is null) return;
            OnClickAction();
        }

        public bool Contains(Vector2 position) => Canvas.Contains(position);
    }
}
