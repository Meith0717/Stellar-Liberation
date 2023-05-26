using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.ComponentModel;
using static Galaxy_Explovive.Core.UserInterface.UiWidgets.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiButton : UiElement
    {
        public RootSide Side = RootSide.None;
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;
        public Action OnKlick = null;
        public MouseActionType MouseActionType = MouseActionType.LeftClickReleased;

        private readonly float mRelativeW;
        private readonly float mRelativeH;
        private readonly Texture2D mTexture;
        private readonly float mTextureAspectRatio;
        private float mTargetAspetRatio;
        private bool mHover;

        public UiButton(UiFrame root, TextureManager textureManager, string texture, float scale) : base(root, textureManager)
        {
            mTexture = mTextureManager.GetTexture(texture);
            mTextureAspectRatio = mTexture.Width / mTexture.Height;
            Rectangle rootRect = Canvas.GetRootRectangle().ToRectangle();
            mRelativeH = mTexture.Height / (float)rootRect.Height * scale;
            mRelativeW = mTexture.Width / (float)rootRect.Width * scale;
        }

        public override void Draw()
        {
            mTextureManager.SpriteBatch.Draw(mTexture, Canvas.ToRectangle(), mHover ? Color.DarkGray : Color.White);
        }

        public override void OnResolutionChanged()
        {
            Canvas.RelativeX = RelativX;
            Canvas.RelativeY = RelativY;
            Canvas.Side = Side;
            Rectangle root = Canvas.GetRootRectangle().ToRectangle();
            mTargetAspetRatio = root.Width * mRelativeW / root.Height * mRelativeH;
            if (mTargetAspetRatio > mTextureAspectRatio)
            {
                Canvas.Height = root.Height * mRelativeH;
                Canvas.Width = Canvas.Height * mTextureAspectRatio;
            }
            else
            {
                Canvas.Width = root.Width * mRelativeW;
                Canvas.Height = Canvas.Width / mTextureAspectRatio;
            }
            Canvas.OnResolutionChanged();
        }

        public override void Update(InputState inputState)
        {
            mHover = false;
            if (Canvas.ToRectangle().Contains(inputState.mMousePosition))
            {
                mHover = true;
                if (inputState.mMouseActionType == MouseActionType)
                {
                    if (OnKlick == null) { return; }
                    OnKlick();
                }
            }
        }
    }
}
