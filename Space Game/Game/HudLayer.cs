using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.NewUi;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Menue.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Game
{
    public class HudLayer : Layer
    {
        private GameLayer mGameLayer;
        private MyUiText mGameTimeText;
        private MyUiSprite mLevelSprite;
        private MyUiText mLevelText;
        private MyUiSprite mAlloySprite;
        private MyUiText mAlloyText;
        private MyUiSprite mEnergySprite;
        private MyUiText mEnergyText;
        private MyUiSprite mMineralsSprite;
        private MyUiText mMineralsText;

        private MyUiSprite mMenueButton;

        public HudLayer(Game1 game, GameLayer gameLayer) : base(game)
        {
            UpdateBelow = true;
            mGameLayer = gameLayer;
            mGameTimeText = new(5, 5, "");
            mLevelSprite = new(150, 5, "level") { Scale=0.32f};
            mLevelText = new(170, 5, "");
            mMenueButton = new(mGraphicsDevice.Viewport.Width - 50, mGraphicsDevice.Viewport.Height - 35, "menue")
            {
                Scale = 0.4f,
                OnClickAction = Pause
            };
            mAlloySprite = new(mGraphicsDevice.Viewport.Width - 500, 5, "alloys");
            mAlloyText = new(mGraphicsDevice.Viewport.Width - 450, 5, "1/100");
            mEnergySprite = new(mGraphicsDevice.Viewport.Width - 350, 5, "energy");
            mEnergyText = new(mGraphicsDevice.Viewport.Width - 300, 5, "1/100");
            mMineralsSprite = new(mGraphicsDevice.Viewport.Width - 200, 5, "minerals");
            mMineralsText = new(mGraphicsDevice.Viewport.Width - 150, 5, "1/100");
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mGameTimeText.Draw(mTextureManager);
            mLevelSprite.Draw(mTextureManager);
            mLevelText.Draw(mTextureManager);
            mAlloySprite.Draw(mTextureManager);
            mAlloyText.Draw(mTextureManager);
            mEnergySprite.Draw(mTextureManager);
            mEnergyText.Draw(mTextureManager);
            mMineralsSprite.Draw(mTextureManager);
            mMineralsText.Draw(mTextureManager);
            mMenueButton.Draw(mTextureManager);
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mMenueButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 50, mGraphicsDevice.Viewport.Height - 35);
            mAlloySprite.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 500, 5);
            mAlloyText.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 450, 5);
            mEnergySprite.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 350, 5);
            mEnergyText.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 300, 5);
            mMineralsSprite.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 200, 5);
            mMineralsText.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 150, 5);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mGameTimeText.Text = MyUtility.ConvertSecondsToTimeUnits((int)mGameLayer.GameTime);
            mMenueButton.Update(mTextureManager, inputState);
            mLevelText.Text = "1";
            mAlloyText.Text = "1/100";
            mEnergyText.Text = "1/100";
            mMineralsText.Text = "1/100";
            if (inputState.mActionList.Contains(ActionType.ESC)) { Pause(); }
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mGame));
        }

        private void Deselect()
        {
            mGameLayer.SelectObject = null;
        }

        private void Track()
        {
            if (mGameLayer.SelectObject is Cargo)
            {
                Cargo ship = (Cargo)mGameLayer.SelectObject;
                ship.mTrack = true;
            }
        }
    }
}
