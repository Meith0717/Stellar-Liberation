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
using StellarLiberation.Game.Core.UserInterface.UiElements;

namespace StellarLiberation.Game.Layers
{
    public class SettingsLayer : Layer
    {
        private readonly UiFrame mMainFrame;
        private readonly UiSlider mMusicSlider;
        private readonly UiSlider mSfxSlider;

        private readonly UiVariableSelector<string> mResolutionSelector;

        public SettingsLayer(bool showBgImage) : base(false)
        {
            mMainFrame = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent };
            if (showBgImage) mMainFrame.Alpha = .8f;
            if (showBgImage) mMainFrame.AddChild(new UiSprite(MenueSpriteRegistries.menueBackground) { FillScale = FillScale.FillIn, Anchor = Anchor.Center });


            var settingsFrame = new UiFrame() { RelHeight = .8f, RelWidth = .5f, Anchor = Anchor.Center};
            mMainFrame.AddChild(settingsFrame);

            var grid = new UiGrid(4, 15) { RelHeight = .9f, RelWidth = .9f, Anchor = Anchor.Center };
            settingsFrame.AddChild(grid);

            grid.Set(0, 0, new UiText(FontRegistries.subTitleFont, "Audio") { Anchor = Anchor.W });
            grid.Set(0, 1, new UiText(FontRegistries.textFont, "Music") { Anchor = Anchor.Center });
            grid.Set(0, 2, new UiText(FontRegistries.textFont, "SFX") { Anchor = Anchor.Center });
            grid.Set(2, 0, new UiText(FontRegistries.subTitleFont, "Video") { Anchor = Anchor.W });
            grid.Set(2, 1, new UiText(FontRegistries.textFont, "Resolution") { Anchor = Anchor.Center });

            mMusicSlider = new(MusicManager.Instance.OverallVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mSfxSlider = new(SoundEffectManager.Instance.OverallVolume) { RelWidth = 1, Anchor = Anchor.CenterH };
            mResolutionSelector = new UiVariableSelector<string>(new()) { OnClickAction = () => LayerManager.ResolutionManager.Apply(mResolutionSelector.Value), RelWidth = 1, Anchor = Anchor.CenterH }; 
            grid.Set(1, 1, mMusicSlider);
            grid.Set(1, 2, mSfxSlider);
            grid.Set(3, 1, mResolutionSelector);

            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Back") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => LayerManager.PopLayer() });
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager)
        {
            base.Initialize(game1, layerManager, graphicsDevice, persistanceManager);
            mResolutionSelector.AddRange(LayerManager.ResolutionManager.Resolutions);
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
            inputState.DoAction(ActionType.ESC, () => LayerManager.PopLayer());
            mMainFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
            MusicManager.Instance.ChangeOverallVolume(mMusicSlider.Value);
            SoundEffectManager.Instance.ChangeOverallVolume(mSfxSlider.Value);
        }
    }
}
