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
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    internal class SettingsLayer : Layer
    {
        private readonly UiLayer mMainFrame;
        private readonly UiSlider mMusicSlider;
        private readonly UiSlider mSfxSlider;

        private readonly UiVariableSelector<string> mResolutionSelector;

        public SettingsLayer(bool showBgImage) : base(false)
        {
            mMainFrame = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent };
            if (showBgImage) mMainFrame.Alpha = .8f;
            if (showBgImage) mMainFrame.AddChild(new UiSprite(TextureRegistries.gameBackground) { FillScale = FillScale.X });

            mMainFrame.AddChild(new UiButton(TextureRegistries.button, "< Back") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.PopLayer() });

            var settingsFrame = new UiFrame(50) { RelHeight = .8f, RelWidth = .5f, Anchor = Anchor.Center, Color = new(17, 17, 17) };
            mMainFrame.AddChild(settingsFrame);

            // Sound Settings
            settingsFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Audio") { HSpace = 20, RelY = .05f });
            mMusicSlider = new(MusicManager.Instance.OverallVolume);
            mSfxSlider = new(SoundEffectManager.Instance.OverallVolume);
            settingsFrame.AddChild(new UiDescriber("Music", mMusicSlider) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .12f });
            settingsFrame.AddChild(new UiDescriber("Effects", mSfxSlider) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .21f });

            // Graphics Settings
            settingsFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Video ") { HSpace = 20, RelY = .30f });
            mResolutionSelector = new UiVariableSelector<string>(new()) { OnClickAction = () => mLayerManager.mResolutionManager.Apply(mResolutionSelector.Value) }; 
            settingsFrame.AddChild(new UiDescriber("Resolution", mResolutionSelector) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .37f });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mResolutionSelector.AddRange(mLayerManager.mResolutionManager.Resolutions);
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
            inputState.DoAction(ActionType.ESC, () => mLayerManager.PopLayer());
            mMainFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.mResolutionManager.UiScaling);
            MusicManager.Instance.ChangeOverallVolume(mMusicSlider.Value);
            SoundEffectManager.Instance.ChangeOverallVolume(mSfxSlider.Value);
        }
    }
}
