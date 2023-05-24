using System;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Galaxy_Explovive.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Game.Layers
{
    public class HudLayer : Layer
    {
        private UiLayer mTopBar;
        private UiLayer mBottomBar;
        private UiText mGameTimeText;

        public HudLayer ()
        {
            UpdateBelow = true;

            mTopBar = new(null)
            {
                RelativeW = 1,
                Color = Color.Black,
                Alpha = 0.5f,
                Height = 40,
                Side = UiCanvas.RootSide.Top
            };

            mGameTimeText = new(mTopBar) { FontColor = Color.White};

            mBottomBar = new(null)
            {
                RelativeW = 1,
                Height = 40,
                Alpha = 0,
                Side = UiCanvas.RootSide.Bottom
            };

            UiLayer leftButtonLayer = new(mBottomBar)
            {
                Height = 40, Width = 200,
                Alpha = 0,
                Side = UiCanvas.RootSide.Right
            };

            new UiButton(leftButtonLayer, "menueButton", 0.2f)
            {
                RelativX = .90f,
                OnKlick = Pause
            };

            OnResolutionChanged();
        }



        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mTopBar.Draw();
            mBottomBar.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mTopBar.OnResolutionChanged();
            mBottomBar.OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                Pause();
            }
            mGameTimeText.Text = MyUtility.ConvertSecondsToTimeUnits((int)Globals.mGameLayer.GameTime);
            mTopBar.Update(inputState);
            mBottomBar.Update(inputState);
        }

        private void Pause()
        {
            Globals.mLayerManager.AddLayer(new PauseLayer());
        }
    }
}
