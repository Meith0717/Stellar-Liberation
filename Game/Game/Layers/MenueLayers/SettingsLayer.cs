// SettingsLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using System;

namespace StellarLiberation.Game.Layers.MenueLayers
{
    public class SettingsLayer : Layer
    {
        private readonly UiFrame mMainFrame;
        private readonly UiGrid mSettingsGrid;
        private UiSlider mMasterSlider;
        private UiSlider mMusicSlider;
        private UiSlider mSfxSlider;
        private UiVariableSelector<string> mResolutionSelector;
        private UiVariableSelector<float> mParticleMultiplier;
        private UiVariableSelector<bool> mLimitRefreshRate;
        private UiVariableSelector<long> mRefreshRate;
        private UiVariableSelector<bool> mVsync;

        public SettingsLayer(bool showBgImage) : base(false)
        {
            mMainFrame = new() { FillScale = FillScale.Both, Color = Color.Transparent };
            if (showBgImage) mMainFrame.Alpha = .8f;
            if (showBgImage) mMainFrame.AddChild(new UiSprite(MenueSpriteRegistries.menueBackground) { FillScale = FillScale.FillIn, Anchor = Anchor.Center });


            var settingsFrame = new UiFrame() { Width = 1800, Height = 900, Anchor = Anchor.Center };
            mMainFrame.AddChild(settingsFrame);

            mSettingsGrid = new UiGrid(4, 15) { RelHeight = .9f, RelWidth = .9f, Anchor = Anchor.Center };
            settingsFrame.AddChild(mSettingsGrid);

            mSettingsGrid.Set(0, 0, new UiText(FontRegistries.subTitleFont, "Audio") { Anchor = Anchor.Center });
            mSettingsGrid.Set(0, 1, new UiText(FontRegistries.textFont, "Master") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 2, new UiText(FontRegistries.textFont, "Music") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(0, 3, new UiText(FontRegistries.textFont, "SFX") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(2, 0, new UiText(FontRegistries.subTitleFont, "Video") { Anchor = Anchor.Center });
            mSettingsGrid.Set(2, 1, new UiText(FontRegistries.textFont, "Resolution") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(2, 2, new UiText(FontRegistries.textFont, "Limit Refresh Rate") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(2, 3, new UiText(FontRegistries.textFont, "Refresh Rate") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(2, 4, new UiText(FontRegistries.textFont, "Vsync") { Anchor = Anchor.E, HSpace = 20 });
            mSettingsGrid.Set(2, 6, new UiText(FontRegistries.subTitleFont, "Graphics") { Anchor = Anchor.Center });
            mSettingsGrid.Set(2, 7, new UiText(FontRegistries.textFont, "Particle Multiplier") { Anchor = Anchor.E, HSpace = 20 });

            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Back & Save") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = Exit });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, GameSettings gameSettings, ResolutionManager resolutionManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager, gameSettings, resolutionManager);

            mMasterSlider = new(GameSettings.MasterVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mMusicSlider = new(GameSettings.MusicVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mSfxSlider = new(GameSettings.SoundEffectsVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mResolutionSelector = new UiVariableSelector<string>(resolutionManager.Resolutions, GameSettings.Resolution) { RelWidth = 1, Anchor = Anchor.CenterH };
            mParticleMultiplier = new UiVariableSelector<float>(new() { 0, 0.2f, 0.5f, 1f, 2f }, GameSettings.ParticlesMultiplier) { RelWidth = 1, Anchor = Anchor.CenterH };
            mLimitRefreshRate = new(new() { true, false}, false) { RelWidth = 1, Anchor = Anchor.CenterH };
            mRefreshRate = new UiVariableSelector<long>(new() { 30, 60, 120 }, GameSettings.RefreshRate) { RelWidth = 1, Anchor = Anchor.CenterH};
            mVsync = new UiVariableSelector<bool>(new() { true, false}, GameSettings.Vsync) { RelWidth = 1, Anchor = Anchor.CenterH };

            mSettingsGrid.Set(1, 1, mMasterSlider);
            mSettingsGrid.Set(1, 2, mMusicSlider);
            mSettingsGrid.Set(1, 3, mSfxSlider);
            mSettingsGrid.Set(3, 1, mResolutionSelector);
            mSettingsGrid.Set(3, 2, mLimitRefreshRate);
            mSettingsGrid.Set(3, 3, mRefreshRate);
            mSettingsGrid.Set(3, 4, mVsync);
            mSettingsGrid.Set(3, 5, new UiButton(MenueSpriteRegistries.button, "Apply Video") { Anchor = Anchor.Center, TextAllign = TextAllign.Center, OnClickAction = ApplyVideo});
            mSettingsGrid.Set(3, 6, mParticleMultiplier);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            mMainFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, Exit);
            mMainFrame.Update(inputState, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);

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
            GameSettings.LimitRefreshRate = mLimitRefreshRate.Value;
            GameSettings.RefreshRate = mRefreshRate.Value;
            GameSettings.Vsync = mVsync.Value;

            ResolutionManager.Apply(GameSettings.Resolution);
            Game1.IsFixedTimeStep = GameSettings.LimitRefreshRate;
            Game1.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / GameSettings.RefreshRate);
            Game1.GraphicsManager.SynchronizeWithVerticalRetrace = GameSettings.Vsync;
            Game1.GraphicsManager.ApplyChanges();
        }

        private void Exit()
        {
            LayerManager.PopLayer();
            LayerManager.AddLayer(new LoadingLayer());
            PersistanceManager.SaveAsync(PersistanceManager.SettingsSaveFilePath, GameSettings, () => LayerManager.PopLayer(), (e) => throw e);
        }
    }
}
