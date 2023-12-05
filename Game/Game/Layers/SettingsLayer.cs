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
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    internal class SettingsLayer : Layer
    {
        private readonly UiLayer mMainFrame;
        private readonly UiSlider mMusicSlider;
        private readonly UiSlider mSfxSlider;

        public SettingsLayer(bool showBgImage) : base(false)
        {
            mMainFrame = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent };
            if (showBgImage) mMainFrame.Alpha = .8f;
            if (showBgImage) mMainFrame.AddChild(new UiSprite(TextureRegistries.gameBackground) { FillScale = FillScale.X });

            mMainFrame.AddChild(new UiButton(TextureRegistries.button, "< Back") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.PopLayer() });
            // mMainFrame.AddChild(new UiButton(TextureRegistries.button, "Apply") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = ApplyChanges });


            var settingsFrame = new UiLayer() { RelHeight = .8f, RelWidth = .5f, Color = new(11, 15, 20), Anchor = Anchor.Center };
            mMainFrame.AddChild(settingsFrame);

            // Sound Settings
            settingsFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Audio") { HSpace = 20, RelY = .05f });
            mMusicSlider = new(MusicManager.Instance.OverallVolume);
            mSfxSlider = new(SoundEffectManager.Instance.OverallVolume);
            settingsFrame.AddChild(new UiDescriber("Music", mMusicSlider) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .12f });
            settingsFrame.AddChild(new UiDescriber("Effects", mSfxSlider) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .21f });

            // Graphics Settings
            // settingsFrame.AddChild(new UiText(FontRegistries.subTitleFont, "Video ") { HSpace = 20, RelY = .30f });
            // var variableSelector = new UiVariableSelector(new() { "1920x1080", "1080x720", "720x480" });
            // var fullScreenSelector = new UiVariableSelector(new() { "True", "False" });
            // var particleSelector = new UiVariableSelector(new() { "Off", "0.25", "0.5", "0.75", "1" });
            // settingsFrame.AddChild(new UiDescriber("Resolution", variableSelector) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .37f });
            // settingsFrame.AddChild(new UiDescriber("Fullscreen", fullScreenSelector) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .44f });
            // settingsFrame.AddChild(new UiDescriber("Particles", particleSelector) { Height = 50, RelWidth = 1, HSpace = 40, RelY = .51f });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mMainFrame.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mMainFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mMainFrame.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => mLayerManager.PopLayer());
            mMainFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds);
            MusicManager.Instance.ChangeOverallVolume(mMusicSlider.Value);
            SoundEffectManager.Instance.ChangeOverallVolume(mSfxSlider.Value);
        }

        private void ApplyChanges()
        {
        }
    }
}
