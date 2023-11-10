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
        public PauseLayer()
            : base(false) 
        {
            mBackgroundLayer = new() {RelHeight = 1, RelWidth = 1, Color = Color.Black, Alpha = .8f};

            mBackgroundLayer.AddChild(new UiButton(TextureRegistries.button, "Continue") {HSpace = 20, RelY = .4f, OnClickAction = () => mLayerManager.PopLayer() });
            mBackgroundLayer.AddChild(new UiButton(TextureRegistries.button, "Save") { HSpace = 20, RelY = .5f, OnClickAction = null });
            mBackgroundLayer.AddChild(new UiButton(TextureRegistries.button, "Menue")
            {
                HSpace = 20,
                RelY = .6f,
                OnClickAction = () =>
                {
                    mLayerManager.PopLayer(); mLayerManager.PopLayer(); SoundManager.Instance.PlaySound(MusicRegistries.bgMusicMenue, 1, isLooped: true, isBackroungMusic: true);
                }
            });
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
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds);
        }
        
        private void Exit()
        {
            mLayerManager.Exit();
        }
    }
}