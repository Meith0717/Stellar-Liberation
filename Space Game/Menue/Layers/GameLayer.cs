using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Menue.Layers
{
    public class GameLayer : Layer
    {

        private readonly GameState mGameState;
        private readonly MyUiMessageManager mMessageManager;

        private readonly MyUiText mGameTimeText;
        private readonly MyUiSprite mLevelSprite;
        private readonly MyUiText mLevelText;
        private readonly MyUiSprite mAlloySprite;
        private readonly MyUiText mAlloyText;
        private readonly MyUiSprite mEnergySprite;
        private readonly MyUiText mEnergyText;
        private readonly MyUiSprite mMineralsSprite;
        private readonly MyUiText mMineralsText;
        private readonly MyUiSprite mMenueButton;
        private readonly MyUiSprite mInfoButton;
        private readonly MyUiText mSelectObjectText;

        private readonly MyUiSprite mDeselectButton;
        private readonly MyUiSprite mTrackButton;
        private readonly MyUiSprite mStopButton;


        public GameLayer(Game1 app) : base(app)
        {
            mGameState = new(mGraphicsDevice, mTextureManager, mSoundManager);
            GameState loadedGame = (GameState)mSerialize.PopulateObject(mGameState, "save");
            if (loadedGame != null) { mGameState = loadedGame; return; }

            mMessageManager = new(mTextureManager, mGraphicsDevice.Viewport.Width / 2, 50);
            mMessageManager.AddMessage("Hello Welcome to my new game GALAXY EXPLOVIVE", GameGlobals.GameTime);
            mMessageManager.AddMessage("Have fun with the game!!!", GameGlobals.GameTime);
            mMessageManager.AddMessage("________Toggle Shadow Mapping with Key-R________", GameGlobals.GameTime);

            GameGlobals.MessageManager = mMessageManager;

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
            mInfoButton = new(5, mGraphicsDevice.Viewport.Height - 80, "info") { Scale = 0.5f };
            mSelectObjectText = new(50, mGraphicsDevice.Viewport.Height - 72, "") { Color = Globals.MormalColor };
            mDeselectButton = new(5, mGraphicsDevice.Viewport.Height - 35, "deSelect")
            { Scale = 0.5f, Disabled = true, OnClickAction = Deselect };
            mTrackButton = new(60, mGraphicsDevice.Viewport.Height - 35, "target")
            { Scale = 0.5f, Disabled = true, OnClickAction = Track };
            mStopButton = new(115, mGraphicsDevice.Viewport.Height - 35, "stop")
            { Scale = 0.5f, Disabled = true, OnClickAction = Stop };
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mGameState.Draw(spriteBatch);

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
            mStopButton.Draw(mTextureManager);
            GameGlobals.DebugSystem.ShowRenderInfo(mTextureManager, 
                GameGlobals.Camera.Zoom, GameGlobals.WorldMousePosition);
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mGameState.ApplyResolution();
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
            mStopButton.OnResolutionChanged(115, mGraphicsDevice.Viewport.Height - 35);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mGameState.Update(inputState, gameTime);

            mMessageManager.Update(inputState, GameGlobals.GameTime);
            mGameTimeText.Text = MyUtility.ConvertSecondsToGameTimeUnits((int)GameGlobals.GameTime);
            mMenueButton.Update(mTextureManager, inputState);
            mLevelText.Text = "1";
            mAlloyText.Text = "1/100";
            mEnergyText.Text = "1/100";
            mMineralsText.Text = "1/100";
            mSelectObjectText.Text = GameGlobals.SelectObject == null ? "" : GameGlobals.SelectObject.GetType().Name;
            mInfoButton.Hide = GameGlobals.SelectObject == null;
            mInfoButton.Update(mTextureManager, inputState);
            mDeselectButton.Update(mTextureManager, inputState);
            mTrackButton.Update(mTextureManager, inputState);
            mStopButton.Update(mTextureManager, inputState);

            if (inputState.mActionList.Contains(ActionType.ESC)) { Pause(); }
            mDeselectButton.Disabled = GameGlobals.SelectObject == null;
            if (GameGlobals.SelectObject == null) { mTrackButton.Disabled = mStopButton.Disabled = true; return; }
            mTrackButton.Disabled = !typeof(Spacecraft).IsAssignableFrom(GameGlobals.SelectObject.GetType());
            mStopButton.Disabled = !typeof(SpaceShip).IsAssignableFrom(GameGlobals.SelectObject.GetType());
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mApp, mGameState));
        }

        private void Deselect()
        {
            GameGlobals.SelectObject = null;
        }

        private void Track()
        {
            SpaceShip ship = (SpaceShip)GameGlobals.SelectObject;
            switch (ship.TargetObj)
            {
                case null:
                    GameGlobals.Camera.TargetPosition = ship.Position;
                    break;
                case not null:
                    if (ship.Track)
                    {
                        ship.Track = false;
                        GameGlobals.Camera.TargetPosition = ship.TargetObj.Position;
                        break;
                    }
                    ship.Track = true;
                    break;
            }
        }

        private void Stop()
        {
            SpaceShip ship = (SpaceShip)GameGlobals.SelectObject;
            ship.Stop = true;
        }
    }
}
