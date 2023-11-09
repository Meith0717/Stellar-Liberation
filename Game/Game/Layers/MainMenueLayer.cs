// MainMenueLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Core.GameEngine.Content_Management;
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
            SoundManager.Instance.PlaySound(ContentRegistry.bgMusicMenue, 1, isLooped:true, isBackroungMusic: true);

            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mFrame.AddChild(new UiSprite(ContentRegistry.menueBackground) { FillScale = FillScale.X });

            mFrame.AddChild(new UiButton(ContentRegistry.button, "New Game") { VSpace = 20, HSpace = 20, RelY = .5f, OnClickAction = StartGame });
            mFrame.AddChild(new UiButton(ContentRegistry.button, "Continue") { VSpace = 20, HSpace = 20, RelY = .6f, OnClickAction = null });
            mFrame.AddChild(new UiButton(ContentRegistry.button, "Settings") { VSpace = 20, HSpace = 20, RelY = .7f, OnClickAction = () => mLayerManager.AddLayer(new TestLayer(false))});
            mFrame.AddChild(new UiButton(ContentRegistry.button, "Credits") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = null });
            mFrame.AddChild(new UiButton(ContentRegistry.button, "Exit Game") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.Exit() });

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

        private void StartGame()
        {
            mLayerManager.AddLayer(new GameLayer());
            SoundManager.Instance.StopBackgroundMusic(ContentRegistry.bgMusicMenue);
        }
    }
}
