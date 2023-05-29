using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
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

        public UiButton(UiFrame root, TextureManager textureManager, GraphicsDevice graphicsDevice, string texture, float scale) 
            : base(root, textureManager, graphicsDevice)
        {
            mTexture = mTextureManager.GetTexture(texture);
            mTextureAspectRatio = mTexture.Width / mTexture.Height;
            Rectangle rootRect = mCanvas.GetRootRectangle(mGraphicsDevice).ToRectangle();
            mRelativeH = mTexture.Height / (float)rootRect.Height * scale;
            mRelativeW = mTexture.Width / (float)rootRect.Width * scale;
        }

        public override void Draw()
        {
            mTextureManager.SpriteBatch.Draw(mTexture, mCanvas.ToRectangle(), mHover ? Color.DarkGray : Color.White);
        }

        public override void OnResolutionChanged()
        {
            mCanvas.RelativeX = RelativX;
            mCanvas.RelativeY = RelativY;
            mCanvas.Side = Side;
            Rectangle root = mCanvas.GetRootRectangle(mGraphicsDevice).ToRectangle();
            mTargetAspetRatio = root.Width * mRelativeW / root.Height * mRelativeH;
            if (mTargetAspetRatio > mTextureAspectRatio)
            {
                mCanvas.Height = root.Height * mRelativeH;
                mCanvas.Width = mCanvas.Height * mTextureAspectRatio;
            }
            else
            {
                mCanvas.Width = root.Width * mRelativeW;
                mCanvas.Height = mCanvas.Width / mTextureAspectRatio;
            }
            mCanvas.OnResolutionChanged(mGraphicsDevice);
        }

        public override void Update(InputState inputState)
        {
            mHover = false;
            if (mCanvas.ToRectangle().Contains(inputState.mMousePosition))
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
