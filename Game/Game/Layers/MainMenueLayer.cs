// MainMenueLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    internal class MainMenueLayer : Layer
    {
        private UiLayer mFrame;
        private ButtonInputTracer mButtonHoverTracer;

        public MainMenueLayer() : base(false)
        {
            MusicManager.Instance.PlayMusic(MusicRegistries.bgMusicMenue);

            mButtonHoverTracer = new();
            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mFrame.AddChild(new UiSprite(TextureRegistries.gameBackground) { FillScale = FillScale.Both });

            mFrame.AddChild(new UiText(FontRegistries.titleFont, "Stellar\nLiberation") { Anchor = Anchor.NW, HSpace = 20, VSpace = 20 });

            var newGame = new UiButton(TextureRegistries.button, "New Game") { VSpace = 20, HSpace = 20, RelY = .5f, OnClickAction = StartGame };
            var _continue = new UiButton(TextureRegistries.button, "Continue") { VSpace = 20, HSpace = 20, RelY = .6f, OnClickAction = null };
            var settings = new UiButton(TextureRegistries.button, "Settings") { VSpace = 20, HSpace = 20, RelY = .7f, OnClickAction = () => mLayerManager.AddLayer(new SettingsLayer(true)) };
            var copyright = new UiButton(TextureRegistries.copyrightButton, "") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = null };
            var exitGame = new UiButton(TextureRegistries.button, "Exit Game") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.Exit() };

            mFrame.AddChild(newGame); mButtonHoverTracer.AddButton(newGame);
            mFrame.AddChild(_continue); mButtonHoverTracer.AddButton(_continue);
            mFrame.AddChild(settings); mButtonHoverTracer.AddButton(settings);
            mFrame.AddChild(exitGame); mButtonHoverTracer.AddButton(exitGame);
            mFrame.AddChild(copyright); mButtonHoverTracer.AddButton(copyright);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mFrame.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy() 
        { 
            SoundEffectManager.Instance.StopAllSounds();
            MusicManager.Instance.StopAllMusics();
        }

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
            inputState.DoAction(ActionType.ESC, mLayerManager.Exit);
            mButtonHoverTracer.Trace(inputState);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds);
        }

        private void StartGame()
        {
            MusicManager.Instance.StopAllMusics();
            mLayerManager.AddLayer(new GameLayer());
        }
    }
}
