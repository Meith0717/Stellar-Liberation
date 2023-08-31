using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.UserInterface;
using CelestialOdyssey.Game.Core.UserInterface.Messages;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Game.Layers
{
    public class HudLayer : Layer
    {
        private readonly GameEngine.GameEngine mEngine;
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


        public HudLayer(Game1 app, GameEngine.GameEngine engine, GameState gameState) : base(app)
        {
            UpdateBelow = true;
            mMessageManager = new(mGraphicsDevice.Viewport.Width / 2, 50);
            mEngine = engine;
            mGameState = gameState;

            mGameTimeText = new(5, 5, "");
            mLevelSprite = new(150, 5, "level") { Scale = 0.32f };
            mLevelText = new(170, 5, "");
            mMenueButton = new(mGraphicsDevice.Viewport.Width - 50, mGraphicsDevice.Viewport.Height - 35, "menue")
            { Scale = 0.5f, OnClickAction = Pause, Color = Color.OrangeRed };
            mAlloySprite = new(mGraphicsDevice.Viewport.Width - 305, 5, "alloys");
            mAlloyText = new(mGraphicsDevice.Viewport.Width - 255, 5, "1/100");
            mEnergySprite = new(mGraphicsDevice.Viewport.Width - 205, 5, "energy");
            mEnergyText = new(mGraphicsDevice.Viewport.Width - 155, 5, "1/100");
            mMineralsSprite = new(mGraphicsDevice.Viewport.Width - 105, 5, "minerals");
            mMineralsText = new(mGraphicsDevice.Viewport.Width - 55, 5, "1/100");
            mInfoButton = new(5, mGraphicsDevice.Viewport.Height - 80, "info")
            { Scale = 0.5f, Color = Color.OrangeRed };
            mSelectObjectText = new(50, mGraphicsDevice.Viewport.Height - 72, "") { Color = Globals.MormalColor };
            mDeselectButton = new(5, mGraphicsDevice.Viewport.Height - 35, "deSelect")
            { Scale = 0.5f, Disabled = true, OnClickAction = Deselect, Color = Color.OrangeRed };
            mTrackButton = new(60, mGraphicsDevice.Viewport.Height - 35, "target")
            { Scale = 0.5f, Disabled = true, OnClickAction = Track, Color = Color.OrangeRed };
            mStopButton = new(115, mGraphicsDevice.Viewport.Height - 35, "stop")
            { Scale = 0.5f, Disabled = true, OnClickAction = Stop, Color = Color.OrangeRed };
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mMessageManager.Draw();
            mGameTimeText.Draw();
            mLevelSprite.Draw();
            mLevelText.Draw();
            mAlloySprite.Draw();
            mAlloyText.Draw();
            mEnergySprite.Draw();
            mEnergyText.Draw();
            mMineralsSprite.Draw();
            mMineralsText.Draw();
            mMenueButton.Draw();
            mInfoButton.Draw();
            mSelectObjectText.Draw();
            mDeselectButton.Draw();
            mTrackButton.Draw();
            mStopButton.Draw();
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
            mStopButton.OnResolutionChanged(115, mGraphicsDevice.Viewport.Height - 35);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mMessageManager.Update(inputState, mEngine.ActiveGameTime);
            mGameTimeText.Text = Utility.ConvertSecondsToGameTimeUnits((int)mEngine.ActiveGameTime / 1000);
            mMenueButton.Update(inputState);
            mLevelText.Text = "1";
            mAlloyText.Text = "1/100";
            mEnergyText.Text = "1/100";
            mMineralsText.Text = "1/100";
            mInfoButton.Hide = false;
            mInfoButton.Update(inputState);
            mDeselectButton.Update(inputState);
            mTrackButton.Update(inputState);
            mStopButton.Update(inputState);
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mApp, mGameState));
        }

        private void Deselect()
        {
        }

        private void Track()
        {
        }

        private void Stop()
        {
        }
    }
}
