﻿// SettingsLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Layers.MenueLayers
{
    public class SettingsLayer : Layer
    {
        private readonly UiCanvas mMainFrame;
        private readonly UiGrid mSettingsGrid;
        private readonly UiSlider mMasterSlider;
        private readonly UiSlider mMusicSlider;
        private readonly UiSlider mSfxSlider;
        private readonly UiVariableSelector<string> mResolutionSelector;
        private readonly UiVariableSelector<float> mParticleMultiplier;
        private readonly UiVariableSelector<string> mRefreshRate;
        private readonly UiCheckBox mVsync;

        public SettingsLayer(Game1 game1) : base(game1, false)
        {
            mMainFrame = new() { FillScale = FillScale.Both, Alpha = 0 };

            UiCanvas settingsFrame; mMainFrame.AddChild(settingsFrame = new() { Width = 1000, Height = 900, Anchor = Anchor.Center });
            settingsFrame.AddChild(new UiText("neuropolitical", "Settings", .1f) { Anchor = Anchor.NW, HSpace = 20, VSpace = 20 });
            settingsFrame.AddChild(mSettingsGrid = new UiGrid(new List<double>() { 0.3, 0.7 }, Enumerable.Repeat(1d / 15, 15).ToList()) { RelHeight = .9f, RelWidth = .9f, Anchor = Anchor.S, VSpace = 10 });

            mSettingsGrid.Set(0, 0, new UiText("neuropolitical", "Audio", .1f) { Anchor = Anchor.Center });
            mSettingsGrid.Set(0, 1, new UiText("neuropolitical", "Master", .1f) { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 2, new UiText("neuropolitical", "Music", .1f) { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 3, new UiText("neuropolitical", "SFX", .1f) { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 4, new UiText("neuropolitical", "Video", .1f) { Anchor = Anchor.Center });
            mSettingsGrid.Set(0, 5, new UiText("neuropolitical", "Resolution", .1f) { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 6, new UiText("neuropolitical", "Refresh Rate", .1f) { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 7, new UiText("neuropolitical", "Vsync", .1f) { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 9, new UiText("neuropolitical", "Graphics", .1f) { Anchor = Anchor.Center });
            mSettingsGrid.Set(0, 10, new UiText("neuropolitical", "Particle Multiplier", .1f) { Anchor = Anchor.E, HSpace = 20 });

            settingsFrame.AddChild(new UiButton("button", "Back & Save") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = Exit });
            settingsFrame.AddChild(new UiButton("button", "Start Benchmark") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE });
            mMasterSlider = new(GameSettings.MasterVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mMusicSlider = new(GameSettings.MusicVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mSfxSlider = new(GameSettings.SoundEffectsVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mResolutionSelector = new UiVariableSelector<string>(ResolutionManager.Resolutions, GameSettings.Resolution) { RelWidth = 1, Anchor = Anchor.CenterH };
            mParticleMultiplier = new UiVariableSelector<float>(new() { 0, 0.2f, 0.5f, 1f, 2f }, GameSettings.ParticlesMultiplier) { RelWidth = 1, Anchor = Anchor.CenterH };
            mRefreshRate = new UiVariableSelector<string>(new() { "30", "60", "75", "120", "Unlimit" }, GameSettings.RefreshRate.ToString()) { RelWidth = 1, Anchor = Anchor.CenterH };
            mVsync = new UiCheckBox(GameSettings.Vsync) { RelWidth = 1, Anchor = Anchor.Center };

            mSettingsGrid.Set(1, 1, mMasterSlider);
            mSettingsGrid.Set(1, 2, mMusicSlider);
            mSettingsGrid.Set(1, 3, mSfxSlider);
            mSettingsGrid.Set(1, 5, mResolutionSelector);
            mSettingsGrid.Set(1, 6, mRefreshRate);
            mSettingsGrid.Set(1, 7, mVsync);
            mSettingsGrid.Set(1, 8, new UiButton("button", "Apply Video") { Anchor = Anchor.Center, OnClickAction = ApplyVideo });
            mSettingsGrid.Set(1, 10, mParticleMultiplier);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            mMainFrame.Draw();
            spriteBatch.End();
        }

        public override void ApplyResolution()
        {
            mMainFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
            base.ApplyResolution();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, Exit);
            mMainFrame.Update(inputState, gameTime);

            GameSettings.MasterVolume = mMasterSlider.Value;
            GameSettings.MusicVolume = mMusicSlider.Value;
            GameSettings.SoundEffectsVolume = mSfxSlider.Value;

            MusicManager.Instance.ChangeOverallVolume(GameSettings.MasterVolume, GameSettings.MusicVolume);
            SoundEffectManager.Instance.ChangeOverallVolume(GameSettings.MasterVolume, GameSettings.SoundEffectsVolume);
            GameSettings.ParticlesMultiplier = mParticleMultiplier.Value;
        }

        private void ApplyVideo()
        {
            GameSettings.Resolution = mResolutionSelector.Value;
            GameSettings.RefreshRate = mRefreshRate.Value;
            GameSettings.Vsync = mVsync.Value;

            ResolutionManager.Apply(GameSettings.Resolution);
            if (Game1.IsFixedTimeStep = long.TryParse(GameSettings.RefreshRate, out long rate))
                Game1.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / rate);
            Game1.GraphicsManager.SynchronizeWithVerticalRetrace = GameSettings.Vsync;
            Game1.GraphicsManager.ApplyChanges();
        }

        private void Exit()
        {
            LayerManager.PopLayer();
            LayerManager.AddLayer(new LoadingLayer(Game1, null));
            PersistanceManager.SaveAsync(PersistanceManager.SettingsSaveFilePath, GameSettings, () => LayerManager.PopLayer(), (e) => throw e);
        }
    }
}
