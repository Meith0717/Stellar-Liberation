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
            Canvas.CenterX = rootRectangle.Width * RelativX + rootRectangle.X;
            Canvas.CenterY = rootRectangle.Height * RelativY + rootRectangle.Y;
            Canvas.Width = mTexture.Width * Scale; 
            Canvas.Height = mTexture.Height * Scale;
            Canvas.Fill = Fill;
            Canvas.OnResolutionChanged();
        }

        public override void Update(InputState inputState)
        {
        }
    }
}