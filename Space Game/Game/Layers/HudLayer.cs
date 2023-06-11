using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Menue.Layers;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Game.Layers
{
    public class HudLayer : Layer
    {
        private MyUiMessageManager mMessageManager;

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
        private MyUiSprite mInfoButton;
        private MyUiText mSelectObjectText;

        private MyUiSprite mDeselectButton;
        private MyUiSprite mTrackButton;

        public HudLayer(Game1 game, GameLayer gameLayer) : base(game)
        {
            UpdateBelow = true;
            mGameLayer = gameLayer;
            mMessageManager = new(mTextureManager, mGraphicsDevice.Viewport.Width / 2, 50);
            mMessageManager.AddMessage("Hi Welcome to my New Game GALAXY EXPLOVIVE", mGameLayer.GameTime);
            mMessageManager.AddMessage("Here New Messages will apppear", mGameLayer.GameTime);
            mMessageManager.AddMessage("Good Game!!!", mGameLayer.GameTime);

            mGameLayer.mMessageManager = mMessageManager;

            mGameTimeText = new(5, 5, "");
            mLevelSprite = new(150, 5, "level") { Scale = 0.32f };
            mLevelText = new(170, 5, "");
            mMenueButton = new(mGraphicsDevice.Viewport.Width - 50, mGraphicsDevice.Viewport.Height - 35, "menue")
            {
                Scale = 0.5f,
                OnClickAction = Pause,
            };
            mAlloySprite = new(mGraphicsDevice.Viewport.Width - 305, 5, "alloys");
            mAlloyText = new(mGraphicsDevice.Viewport.Width - 255, 5, "1/100");
            mEnergySprite = new(mGraphicsDevice.Viewport.Width - 205, 5, "energy");
            mEnergyText = new(mGraphicsDevice.Viewport.Width - 155, 5, "1/100");
            mMineralsSprite = new(mGraphicsDevice.Viewport.Width - 105, 5, "minerals");
            mMineralsText = new(mGraphicsDevice.Viewport.Width - 55, 5, "1/100");
            mInfoButton = new(5, mGraphicsDevice.Viewport.Height - 80, "info") { Scale=0.5f};
            mSelectObjectText = new(50, mGraphicsDevice.Viewport.Height - 72, "") { Color=Globals.MormalColor};
            mDeselectButton = new(5, mGraphicsDevice.Viewport.Height - 35, "deSelect") 
            { Scale = 0.5f, Disabled=true, OnClickAction=Deselect};
            mTrackButton = new(60, mGraphicsDevice.Viewport.Height - 35, "track") 
            { Scale = 0.5f, Disabled = true, OnClickAction = Track };
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mMessageManager.Draw();
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
            mInfoButton.Draw(mTextureManager);
            mSelectObjectText.Draw(mTextureManager);
            mDeselectButton.Draw(mTextureManager);
            mTrackButton.Draw(mTextureManager);
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mMessageManager.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2, 50);
            mMenueButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 50, mGraphicsDevice.Viewport.Height - 35);
            mAlloySprite.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 305, 5);
            mAlloyText.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 255, 5);
            mEnergySprite.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 205, 5);
            mEnergyText.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 155, 5);
            mMineralsSprite.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 105, 5);
            mMineralsText.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 55, 5);
            mInfoButton.OnResolutionChanged(5, mGraphicsDevice.Viewport.Height - 80);
            mSelectObjectText.OnResolutionChanged(50, mGraphicsDevice.Viewport.Height - 72);
            mDeselectButton.OnResolutionChanged(5, mGraphicsDevice.Viewport.Height - 35);
            mTrackButton.OnResolutionChanged(60, mGraphicsDevice.Viewport.Height - 35);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mMessageManager.Update(mGameLayer.GameTime);
            mGameTimeText.Text = MyUtility.ConvertSecondsToGameTimeUnits((int)mGameLayer.GameTime);
            mMenueButton.Update(mTextureManager, inputState);
            mLevelText.Text = "1";
            mAlloyText.Text = "1/100";
            mEnergyText.Text = "1/100";
            mMineralsText.Text = "1/100";
            mSelectObjectText.Text = (mGameLayer.SelectObject == null)? "" : mGameLayer.SelectObject.GetType().Name;
            mInfoButton.Hide = mGameLayer.SelectObject == null;
            mInfoButton.Update(mTextureManager, inputState);
            mDeselectButton.Update(mTextureManager, inputState);
            mTrackButton.Update(mTextureManager, inputState);
            if (inputState.mActionList.Contains(ActionType.ESC)) { Pause(); }
            mDeselectButton.Disabled = mGameLayer.SelectObject == null;
            if (mGameLayer.SelectObject == null) return;     
            mTrackButton.Disabled = !typeof(Spacecraft).IsAssignableFrom(mGameLayer.SelectObject.GetType());
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
            SpaceShip ship = (SpaceShip)mGameLayer.SelectObject;
            switch (ship.TargetPosition)
            {
                case null:
                    mGameLayer.mCamera.TargetPosition = ship.TargetPosition.Position;
                    break;
                case not null:
                    if (Vector2.Distance(ship.TargetPosition.Position, mGameLayer.mCamera.Position) < 100)
                    {
                        ship.Track = true;
                        return;
                    }
                    if (ship.Track) 
                    {
                        ship.Track = false;
                        return;
                    }
                    mGameLayer.mCamera.TargetPosition = ship.TargetPosition.Position;
                    break;
            }

        }
    }
}
