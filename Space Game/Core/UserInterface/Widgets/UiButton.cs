using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using static Galaxy_Explovive.Core.UserInterface.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiButton : UiElement
    {
        public RootFill Fill = RootFill.Fix;
        public float Scale = 1f;
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;
        public Action OnKlick = null;

        private Texture2D mTexture;
        private bool mHover;

        public UiButton(UiLayer root, string texture) : base(root)
        {
            mTexture = TextureManager.Instance.GetTexture(texture);
        }

        public override void Draw()
        {
            TextureManager.Instance.GetSpriteBatch().Draw(mTexture, Canvas.ToRectangle(), mHover ? Color.DarkGray : Color.White);
        }

        public override void OnResolutionChanged()
        {
            Rectangle rootRectangle = Canvas.GetRootRectangle().ToRectangle();
            Canvas.CenterX = rootRectangle.Width * RelativX;
            Canvas.CenterY = rootRectangle.Height * RelativY;
            Canvas.Width = mTexture.Width * Scale;
            Canvas.Height = mTexture.Height * Scale;
            Canvas.Fill = Fill;
            Canvas.OnResolutionChanged();
        }

        public override void Update(InputState inputState)
        {
            mHover = false;
            if (Canvas.ToRectangle().Contains(inputState.mMousePosition))
            {
                mHover= true;
                if (inputState.mMouseActionType == MouseActionType.LeftClick)
                {
                    if (OnKlick == null) { return; }
                    OnKlick();
                }
            }
        }

    }
}
