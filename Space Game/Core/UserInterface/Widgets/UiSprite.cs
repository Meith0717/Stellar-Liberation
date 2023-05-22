﻿using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using static Galaxy_Explovive.Core.UserInterface.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiSprite : UiElement
    {
        public RootFill Fill = RootFill.Fix;
        public float Scale = 1f;
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;

        private Texture2D mTexture;

        public UiSprite(UiLayer root, string texture) : base(root) 
        {
            mTexture = TextureManager.Instance.GetTexture(texture);
        }

        public override void Draw()
        {
            TextureManager.Instance.GetSpriteBatch().Draw(mTexture, Canvas.ToRectangle(), Color.White);
        }

        public override void OnResolutionChanged()
        {
            Rectangle rootRectangle = Canvas.GetRootRectangle().ToRectangle();
            Canvas.RelativeX = RelativX;
            Canvas.RelativeY = RelativY;
            Canvas.Width = mTexture.Width;
            Canvas.Height = mTexture.Height;
            Canvas.Fill = Fill;
            Canvas.OnResolutionChanged();
        }

        public override void Update(InputState inputState)
        {
        }
    }
}
