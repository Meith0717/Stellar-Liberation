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
            mSettingsGrid.Set(0, 1, new UiText(FontRegistries.textFont, "Master") { Anchor = Anchor.Center });
            mSettingsGrid.Set(0, 2, new UiText(FontRegistries.textFont, "Music") { Anchor = Anchor.Center });
            mSettingsGrid.Set(0, 3, new UiText(FontRegistries.textFont, "SFX") { Anchor = Anchor.Center });
            mSettingsGrid.Set(2, 0, new UiText(FontRegistries.subTitleFont, "Video") { Anchor = Anchor.Center });
            mSettingsGrid.Set(2, 1, new UiText(FontRegistries.textFont, "Resolution") { Anchor = Anchor.Center });

            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Back") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = Exit });
            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Apply") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = Apply });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, GameSettings gameSettings, ResolutionManager resolutionManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager, gameSettings, resolutionManager);

            mMasterSlider = new(GameSettings.MasterVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mMusicSlider = new(GameSettings.MusicVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mSfxSlider = new(GameSettings.SoundEffectsVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mResolutionSelector = new UiVariableSelector<string>(resolutionManager.Resolutions) { RelWidth = 1, Anchor = Anchor.CenterH };

            mSettingsGrid.Set(1, 1, mMasterSlider);
            mSettingsGrid.Set(1, 2, mMusicSlider);
            mSettingsGrid.Set(1, 3, mSfxSlider);
            mSettingsGrid.Set(3, 1, mResolutionSelector);
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
        }

        private void Apply()
        {
            GameSettings.Resolution = mResolutionSelector.Value;
            ResolutionManager.Apply(GameSettings.Resolution);
        }

        private void Exit()
        {
            LayerManager.PopLayer();
            LayerManager.AddLayer(new LoadingLayer());
            PersistanceManager.SaveAsync(PersistanceManager.SettingsSaveFilePath, GameSettings, () => LayerManager.PopLayer(), (e) => throw e);
        }
    }
}
