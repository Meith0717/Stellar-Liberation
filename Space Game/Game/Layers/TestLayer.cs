﻿using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game.Layers
{
    internal class TestLayer : Layer
    {
        private UiLayer mRoot;

        public TestLayer()
        {
            mRoot = new(null, .5f, .5f, .5f, .5f)
            {
                Color = Color.LightBlue,
                Borgerwidth = 100
            };

            new UiLayer(mRoot, .5f, .5f, .5f, .5f)
            {
                Color = Color.LightGreen,
                Borgerwidth = 50
            };

            OnResolutionChanged();
        }

        public override void Destroy()
        {
        }

        public override void Draw()
        {
            mSpriteBatch.Begin();
            mRoot.Draw();
            mSpriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mRoot.OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mRoot.Update(inputState);
        }
    }
}
