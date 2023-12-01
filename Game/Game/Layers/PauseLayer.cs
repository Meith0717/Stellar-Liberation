﻿// PauseLayer.cs 
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
    public class PauseLayer : Layer
    {
        private UiLayer mBackgroundLayer;
        private ButtonInputTracer mButtonInputTracer;

        public PauseLayer()
            : base(false) 
        {
            mBackgroundLayer = new() {RelHeight = 1, RelWidth = 1, Color = Color.Black, Alpha = .8f};
            mButtonInputTracer = new();

            var buttonFrame = new UiLayer() { Anchor = Anchor.Center, Height = 300, Width = 300 , HSpace = 20, VSpace = 20, Alpha = 0 };
            mBackgroundLayer.AddChild(buttonFrame);

            var _continue = new UiButton(TextureRegistries.button, "Resume") { Anchor = Anchor.N, FillScale = FillScale.X, OnClickAction = () => mLayerManager.PopLayer(), TextAllign = TextAllign.Center };
            var save = new UiButton(TextureRegistries.button, "Save") { RelY = .27f, FillScale = FillScale.X, OnClickAction = null, TextAllign = TextAllign.Center };
            var settings = new UiButton(TextureRegistries.button, "Settings") { RelY = .55f, FillScale = FillScale.X, OnClickAction =  () => mLayerManager.AddLayer(new SettingsLayer(false)), TextAllign = TextAllign.Center };
            var menue = new UiButton(TextureRegistries.button, "Menue") { Anchor = Anchor.S, FillScale = FillScale.X, OnClickAction = Menue, TextAllign = TextAllign.Center };

            buttonFrame.AddChild(_continue); mButtonInputTracer.AddButton(_continue); ;
            buttonFrame.AddChild(save); mButtonInputTracer.AddButton(save);
            buttonFrame.AddChild(settings); mButtonInputTracer.AddButton(settings);
            buttonFrame.AddChild(menue); mButtonInputTracer.AddButton(menue);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mBackgroundLayer.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => mLayerManager.PopLayer());
            mButtonInputTracer.Trace(inputState);
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds);
        }
        
        private void Menue()
        {
            mLayerManager.PopLayer(); // Pause Menue
            mLayerManager.PopLayer(); // Hud
            mLayerManager.PopLayer(); // Game
            MusicManager.Instance.StopAllMusics();
            MusicManager.Instance.PlayMusic(MusicRegistries.bgMusicMenue);
        }
    }
}
