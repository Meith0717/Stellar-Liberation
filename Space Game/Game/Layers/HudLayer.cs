using System;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.MyMath;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;

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
                Side = Core.UserInterface.UiCanvas.RootSide.Top
            };

            mGameTimeText = new(mTopBar) { FontColor = Color.White};

            mBottomBar = new(null)
            {
                RelativeW = 1,
                Height = 40,
                Alpha = 0,
                Side = Core.UserInterface.UiCanvas.RootSide.Bottom
            };

            UiLayer leftButtonLayer = new(mBottomBar)
            {
                Height = 40, Width = 200,
                Alpha = 0,
                Side = Core.UserInterface.UiCanvas.RootSide.Right
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

        public override void Draw()
        {
            mSpriteBatch.Begin();
            mTopBar.Draw();
            mBottomBar.Draw();
            mSpriteBatch.End();
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
            mGameTimeText.Text = MyMath.Instance.GetTimeFromSeconds((int)Globals.mGameLayer.GameTime);
            mTopBar.Update(inputState);
            mBottomBar.Update(inputState);
        }

        private void Pause()
        {
            Globals.mLayerManager.AddLayer(new PauseLayer());
        }
    }
}
