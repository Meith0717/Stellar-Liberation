using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Menue.Layers
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
                RelativeW = 1,
                Color = Color.Black,
                Alpha = 0.5f,
                Height = 40,
                Side = UiCanvas.RootSide.Top
            };

            mGameTimeText = new(mTopBar, mTextureManager, mGraphicsDevice) { FontColor = Color.White };

            mBottomBar = new(null, mTextureManager, mGraphicsDevice)
            {
                RelativeW = 1,
                Height = 40,
                Alpha = 0,
                Side = UiCanvas.RootSide.Bottom
            };

            UiFrame leftButtonLayer = new(mBottomBar, mTextureManager, mGraphicsDevice)
            {
                Height = 40,
                Width = 200,
                Alpha = 0,
                Side = UiCanvas.RootSide.Right
            };

            new UiButton(leftButtonLayer, mTextureManager, mGraphicsDevice, "menueButton", 0.2f)
            {
                RelativX = .90f,
                OnKlick = Pause
            };

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
