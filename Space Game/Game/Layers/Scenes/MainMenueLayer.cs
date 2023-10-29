// MainMenueLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Persistance;
using CelestialOdyssey.Game.Core.UserInterface;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CelestialOdyssey.Game.Layers.Scenes
{
    internal class MainMenueLayer : Layer
    {
        private UiLayer mFrame;

        public MainMenueLayer() : base(false)
        {
            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };

            var leftButtonFrame = new UiLayer() { RelHeight = .06f, RelWidth = 1f, Anchor = Anchor.SW, Color = Color.Green, Alpha = 0, HSpace = 20, VSpace = 20 };
            mFrame.AddChild(leftButtonFrame);

            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonNewGame) { FillScale = FillScale.Y, Anchor = Anchor.W, OnClickAction = () => mLayerManager.AddLayer(new GameLayer())});
            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonContinue) { RelX = .2f, FillScale = FillScale.Y , IsDisabled = true});
            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonSettings) { RelX = .4f, FillScale = FillScale.Y, IsDisabled = true });
            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonQuit) { FillScale = FillScale.Y, Anchor = Anchor.E, OnClickAction = () => mLayerManager.Exit()});
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mFrame.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mFrame.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds);
        }
    }
}
