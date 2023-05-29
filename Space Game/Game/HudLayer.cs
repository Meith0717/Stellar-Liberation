using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Menue.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Game
{
    public class HudLayer : Layer
    {
        private readonly UiFrame mTopBar;
        private readonly UiFrame mBottomBar;
        private readonly UiText mGameTimeText;
        private readonly GameLayer mGameLayer;

        public HudLayer(Game1 game, GameLayer gameLayer)
            : base(game)
        {
            UpdateBelow = true;
            mGameLayer = gameLayer;

            mTopBar = new(null, mTextureManager, mGraphicsDevice)
            {
                RelativeW = 1f,
                Color = new Color(66, 73, 73),
                Height = 75,
                EdgeWidth = 50,
                MarginY = -20,
                Side = UiCanvas.RootSide.Top
            };

            mGameTimeText = new(mTopBar, mTextureManager, mGraphicsDevice) 
            { FontColor = Color.White, Side = UiCanvas.RootSide.Left, RelativY = 0.65f, MarginX = 20 };

            mBottomBar = new(null, mTextureManager, mGraphicsDevice)
            { RelativeW = 1, Height = 40, Alpha = 0, Side = UiCanvas.RootSide.Bottom };

            UiFrame leftButtonLayer = new(mBottomBar, mTextureManager, mGraphicsDevice)
            { Height = 40, Width = 200, Alpha = 0, Side = UiCanvas.RootSide.Right };

            _ = new UiButton(leftButtonLayer, mTextureManager, mGraphicsDevice, "menueButton", 0.2f)
            { RelativX = .90f, OnKlick = Pause };

            OnResolutionChanged();
        }

        public override void Destroy() { }

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
            mGameTimeText.Text = MyUtility.ConvertSecondsToTimeUnits((int)mGameLayer.GameTime);
            mTopBar.Update(inputState);
            mBottomBar.Update(inputState);
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mGame));
        }
    }
}
