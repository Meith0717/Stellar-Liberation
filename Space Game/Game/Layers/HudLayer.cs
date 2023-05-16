using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Game.Layers
{
    internal class HudLayer : Layer
    {
        private UiLayer mTopBar;

        public HudLayer ()
        {
            mTopBar = new(null, .5, .5, .5, .5)
            {
                Color = Color.DarkGray,
                Alpha = .5f,
            };

            new UiLayer(mTopBar, .5, .5, .1, .5)
            {
                MinHeight = 50, 
                MaxHeight = 50,
                MinWidth = 50,
                MaxWidth = 50,
                Fill = Core.UserInterface.UiCanvas.RootFill.Fit
            };

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
