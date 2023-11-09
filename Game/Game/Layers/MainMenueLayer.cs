// MainMenueLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.GameEngine.Content_Management;

namespace StellarLiberation.Game.Layers
{
    internal class MainMenueLayer : Layer
    {
        private UiLayer mFrame;

        public MainMenueLayer() : base(false)
        {
            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mFrame.AddChild(new UiSprite(ContentRegistry.menueBackground) { FillScale = FillScale.X });

            var leftButtonFrame = new UiLayer() { RelHeight = .55f, RelWidth = 0.2f, Anchor = Anchor.SW, Color = Color.Green, Alpha = .0f, HSpace = 20, VSpace = 20 };
            mFrame.AddChild(leftButtonFrame);

            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonNewGame) { FillScale = FillScale.X, Anchor = Anchor.N, OnClickAction = () => mLayerManager.AddLayer(new GameLayer()) });
            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonContinue) { RelY = .2f, FillScale = FillScale.X, IsDisabled = true });
            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonSettings) { RelY = .4f, FillScale = FillScale.X, IsDisabled = true });
            leftButtonFrame.AddChild(new UiButton(ContentRegistry.buttonQuit) { FillScale = FillScale.X, Anchor = Anchor.S, OnClickAction = () => mLayerManager.Exit() });

            var topTitleFrame = new UiLayer() { RelHeight = 0.25f, RelWidth = 0.4f, Color = Color.Green, Alpha = .0f, HSpace = 20, VSpace = 20 };
            mFrame.AddChild(topTitleFrame);
            topTitleFrame.AddChild(new UiSprite(ContentRegistry.title) { FillScale = FillScale.X});
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
