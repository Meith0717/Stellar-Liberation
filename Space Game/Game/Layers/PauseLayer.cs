// PauseLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Persistance;
using CelestialOdyssey.Game.Core.UserInterface;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Game.Layers
{
    public class PauseLayer : Layer
    {
        private MyUiFrame mBackgroundLayer;
        private MyUiSprite mContinueButton;
        private MyUiSprite mExitButton;

        public PauseLayer()
            : base(false) { }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mBackgroundLayer = new(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height)
            { Alpha = 0.95f, Color = Color.Black };
            mContinueButton = new(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 - 100,
                ContentRegistry.buttonContinue)
            {
                OnClickAction = mLayerManager.PopLayer,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
            mExitButton = new(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 100,
                ContentRegistry.buttonExitgame)
            {
                OnClickAction = Exit,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            mContinueButton.Draw();
            mExitButton.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mContinueButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 - 100);
            mExitButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 100);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mContinueButton.Update(inputState);
            mExitButton.Update(inputState);
            inputState.DoAction(ActionType.ESC, mLayerManager.PopLayer);
        }

        private void Exit()
        {
            mLayerManager.Exit();
        }
    }
}
