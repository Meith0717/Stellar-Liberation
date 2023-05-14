using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using System;

namespace Galaxy_Explovive.Game.Layers
{
    internal class BuildLayer : Layer
    {

        private UiLayer mRoot;
        private UiLayer mBackButtonLayer;
        private UiButton mBackButton;

        public BuildLayer() 
        {
            UpdateBelow = false;
            mRoot = new(null, .5, .5, 1, 1)
            {
                BgColorAlpha = .8f,
                BgColor = Color.Black
            };
            mBackButtonLayer = new(mRoot, .95, .05, .05, .05)
            {
                BgColorAlpha = 0 
            };
            mBackButton = new(mBackButtonLayer, .5, .5)
            {
                Image = "back",
                OnClickAction = Globals.mLayerManager.PopLayer
            };
            mRoot.AddToChilds(mBackButtonLayer);
            mBackButtonLayer.AddToChilds(mBackButton);
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

