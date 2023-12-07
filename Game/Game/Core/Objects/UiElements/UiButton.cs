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


        public override void Initialize(Rectangle root, float UiScaling) => OnResolutionChanged(root, UiScaling);

        public override void Draw()
        {
            var color = IsDisabled ? Color.DarkGray : IsHover ? Color.MonoGameOrange : Color.White;
            TextureManager.Instance.Draw(mSpriteId, Canvas.Position, Canvas.Bounds.Width, Canvas.Bounds.Height, color);
            TextureManager.Instance.DrawString(FontRegistries.buttonFont, TextPosition, mText, mTextScale, color);
            Canvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float UiScaling)
        {
            var stringDim = TextureManager.Instance.GetFont(FontRegistries.buttonFont).MeasureString(mText);
            var textHeight = Canvas.Center.Y - (stringDim.Y * UiScaling / 2);
            switch (TextAllign)
            {
                case TextAllign.W:
                    TextPosition = new Vector2(Canvas.Bounds.Left, textHeight);
                    break;
                case TextAllign.E:
                    TextPosition = new Vector2(Canvas.Bounds.Right - (stringDim.X * UiScaling), textHeight);
                    break;
                case TextAllign.Center:
                    TextPosition = new Vector2(Canvas.Center.X - (stringDim.X * UiScaling / 2), textHeight);
                    break;
            }
            IsHover = Canvas.Contains(inputState.mMousePosition);
            if (IsHover && !IsDisabled && IsHover != mLastHoverState) SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.hover);
            if (IsHover) inputState.DoAction(ActionType.Select, Click);
            mLastHoverState = IsHover;
        }

        private void Click()
        {
            if (OnClickAction is null) return;
            OnClickAction();
        }

        public override void OnResolutionChanged(Rectangle root, float UiScaling)
        {
            var texture = TextureManager.Instance.GetTexture(mSpriteId);
            mTextScale = UiScaling;
            Width = (int)(texture.Width * UiScaling);
            Height = (int)(texture.Height * UiScaling);
            Canvas.UpdateFrame(root);
        }

        public bool Contains(Vector2 position) => Canvas.Contains(position);
    }
}
