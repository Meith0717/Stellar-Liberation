// UiButton.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
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
            mCanvas.Width = texture.Width;
            mCanvas.Height = texture.Height;
        }


        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
        }

        public override void Draw() 
        {
            var color = IsHover ? Color.MonoGameOrange : Color.White;
            TextureManager.Instance.Draw(mSpriteId, mCanvas.Position, mCanvas.Bounds.Width, mCanvas.Bounds.Height, color);
            TextureManager.Instance.DrawString("button", TextPosition, mText, mTextScale, color);
        } 

        public override void Update(InputState inputState, Rectangle root) 
        {
            IsDisabled = OnClickAction is null;
            var stringDim = TextureManager.Instance.GetFont("button").MeasureString(mText);
            switch (TextAllign)
            {
                case TextAllign.W:
                    TextPosition = new Vector2(mCanvas.Bounds.Left + 5, mCanvas.Position.Y + 10);
                    break;
                case TextAllign.E:
                    TextPosition = new Vector2(mCanvas.Bounds.Right - stringDim.X - 5, mCanvas.Position.Y + 10);
                    break;
                case TextAllign.Center:
                    TextPosition = new Vector2(mCanvas.Center.X - (stringDim.X / 2), mCanvas.Position.Y + 10);
                    break;
            }
            IsHover = mCanvas.Contains(inputState.mMousePosition);
            if (IsHover) inputState.DoAction(ActionType.Select, OnClickAction);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
        }

        public bool Contains(Vector2 position) => mCanvas.Contains(position);
    }
}
