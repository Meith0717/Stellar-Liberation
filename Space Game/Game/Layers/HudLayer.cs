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

        public HudLayer()
        {
            mTopBar = new(null, .5, .95, 1, .1)
            {
                BgColor = Color.White,
                BgColorAlpha = .2,
                MinHeight = 100,
                MaxHeight = 100
            };
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
