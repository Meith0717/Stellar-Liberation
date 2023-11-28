﻿// SettingsLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement;

namespace StellarLiberation.Game.Layers
{
    internal class SettingsLayer : Layer
    {
        private UiLayer mSettingFrame;
        private UiSlider mMusicSlider;
        private UiSlider mSfxSlider;

        public SettingsLayer(bool updateBelow) : base(updateBelow)
        {
            mSettingFrame = new() { RelX = .5f, RelHeight = 1, RelWidth = .5f, HSpace = 100, VSpace = 100, Color = Color.Black, Alpha = .7f };

            mSettingFrame.AddChild(new UiButton(TextureRegistries.button, "< Back") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.PopLayer() });
            mSettingFrame.AddChild(new UiButton(TextureRegistries.button, "Apply") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = ApplyChanges });


            mMusicSlider = new("Music Volume ", MusicManager.Instance.OverallVolume) { Width = 800, RelX = .01f, RelY = .1f};
            mSettingFrame.AddChild(mMusicSlider);
            mSfxSlider = new("SFX Volume       ", SoundEffectManager.Instance.OverallVolume) { Width = 800, RelX = .01f, RelY = .2f };
            mSettingFrame.AddChild(mSfxSlider);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mSettingFrame.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mSettingFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mSettingFrame.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mSettingFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds);
            MusicManager.Instance.ChangeOverallVolume(mMusicSlider.Value);
            SoundEffectManager.Instance.ChangeOverallVolume(mSfxSlider.Value);
        }

        private void ApplyChanges()
        {
        }
    }
}