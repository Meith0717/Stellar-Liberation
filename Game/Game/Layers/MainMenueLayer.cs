// MainMenueLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.UserInterface;
using System.Net.Security;

namespace StellarLiberation.Game.Layers
{
    public class MainMenueLayer : Layer
    {
        private UiLayer mFrame;

        public MainMenueLayer() : base(false)
        {
            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mFrame.AddChild(new UiSprite(TextureRegistries.menueBackground) { FillScale = FillScale.Both });

            mFrame.AddChild(new UiText(FontRegistries.titleFont, "Stellar\nLiberation") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });

            mFrame.AddChild(new UiButton(TextureRegistries.button, "New Game") { VSpace = 20, HSpace = 20, RelY = .5f, OnClickAction = () => mLayerManager.AddLayer(new GameLayer()) });

            mFrame.AddChild(new UiButton(TextureRegistries.button, "Continue") { VSpace = 20, HSpace = 20, RelY = .6f, OnClickAction = () => 
            {
                mLayerManager.AddLayer(new LoadingLayer("Loading"));
                mPersistanceManager.LoadGameLayerAsync((gL) => { mLayerManager.AddLayerFromThread(gL); mLayerManager.PopLayer(); }, (ex) => throw ex);
            }
            });

            mFrame.AddChild(new UiButton(TextureRegistries.button, "Settings") { VSpace = 20, HSpace = 20, RelY = .7f, OnClickAction = () => mLayerManager.AddLayer(new SettingsLayer(true)) });

            mFrame.AddChild(new UiButton(TextureRegistries.copyrightButton, "") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = null });

            mFrame.AddChild(new UiButton(TextureRegistries.button, "Exit Game") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.Exit() });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager);
            mLayerManager.ResolutionManager.GetNativeResolution();
        }

        public override void Destroy()
        {
            SoundEffectManager.Instance.StopAllSounds();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, mLayerManager.Exit);
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.ResolutionManager.UiScaling);
        }

        private void StartGame()
        {
            mLayerManager.AddLayer(new GameLayer());
        }
    }
}
