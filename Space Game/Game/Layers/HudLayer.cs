using System;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game.Layers
{
    internal class HudLayer : Layer
    {
        private UiLayer mTopBar;

        public HudLayer ()
        {
            mTopBar = new(null);
            mTopBar.Alpha = 0.2f;

            UiButton exitButton = new(mTopBar, "Back");
            exitButton.Scale = 0.15f;
            OnResolutionChanged();
        }



        public override void Destroy()
        {
        }

        public override void Draw()
        {
            mSpriteBatch.Begin();
            mTopBar.Draw();
            mSpriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mTopBar.OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mTopBar.Update(inputState);
        }
    }
}
