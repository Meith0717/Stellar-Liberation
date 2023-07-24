﻿using CelestialOdyssey.Core;
using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Core.GameEngine.InputManagement;
using CelestialOdyssey.Core.GameEngine.Utility;
using CelestialOdyssey.Core.GameObject;
using CelestialOdyssey.Core.LayerManagement;
using CelestialOdyssey.Core.UserInterface;
using CelestialOdyssey.Core.UserInterface.Messages;
using CelestialOdyssey.Game;
using CelestialOdyssey.Game.GameObjects.Spacecraft.SpaceShips;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Layers
{
    public class HudLayer : Layer
    {
        private readonly GameEngine mEngine;
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


        public HudLayer(Game1 app, GameEngine engine, GameState gameState) : base(app)
        {
            UpdateBelow = true;
            mMessageManager = new(mTextureManager, mGraphicsDevice.Viewport.Width / 2, 50);
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
            { Scale = 0.5f, Disabled = true, OnClickAction = Stop, Color=Color.OrangeRed};
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
            mStopButton.Draw(mTextureManager);
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
            mMessageManager.Update(inputState, mEngine.GameTime);
            mGameTimeText.Text = Utility.ConvertSecondsToGameTimeUnits((int)mEngine.GameTime/1000);
            mMenueButton.Update(mTextureManager, inputState);
            mLevelText.Text = "1";
            mAlloyText.Text = "1/100";
            mEnergyText.Text = "1/100";
            mMineralsText.Text = "1/100";
            mSelectObjectText.Text = mEngine .SelectObject == null ? "" : mEngine.SelectObject.GetType().Name;
            mInfoButton.Hide = mEngine.SelectObject == null;
            mInfoButton.Update(mTextureManager, inputState);
            mDeselectButton.Update(mTextureManager, inputState);
            mTrackButton.Update(mTextureManager, inputState);
            mStopButton.Update(mTextureManager, inputState);

            if (inputState.mActionList.Contains(ActionType.ESC)) { Pause(); }

            var SelectObjectIsNull = mEngine.SelectObject == null;
            mDeselectButton.Disabled = SelectObjectIsNull;
            mTrackButton.Disabled = SelectObjectIsNull;

            if (SelectObjectIsNull) { mStopButton.Disabled = true; return; }
            mStopButton.Disabled = !typeof(SpaceShip).IsAssignableFrom(mEngine.SelectObject.GetType());
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mApp, mGameState));
        }

        private void Deselect()
        {
            mEngine.SelectObject = null;
        }

        private void Track()
        {
            switch (mEngine.SelectObject)
            {
                case SpaceShip ship:
                    switch (ship.TargetObj)
                    {
                        case null:
                            mEngine.SelectObject.IsTracked = !mEngine.SelectObject.IsTracked;
                            break;
                        case not null:
                            if (mEngine.SelectObject.IsTracked) 
                            {
                                mEngine.Camera.MoveToTarget(ship.TargetObj.Position);
                                mEngine.SelectObject.IsTracked = false;
                                break;
                            }
                            mEngine.SelectObject.IsTracked = !mEngine.SelectObject.IsTracked;
                            break;
                    }
                    break;

                default:
                    mEngine.SelectObject.IsTracked = !mEngine.SelectObject.IsTracked;
                    mEngine.Camera.MoveToTarget(mEngine.SelectObject.Position);
                    break;
            }
        }

        private void Stop()
        {
            SpaceShip ship = (SpaceShip)mEngine.SelectObject;
            ship.Stop = true;
        }
    }
}
