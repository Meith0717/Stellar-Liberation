// PauseLayer.cs 
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
    public class PauseLayer : Layer
    {
        private UiLayer mBackgroundLayer;
        private ButtonInputTracer mButtonInputTracer;

        public PauseLayer()
            : base(false)
        {
            mBackgroundLayer = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent};
            mButtonInputTracer = new();

            var buttonFrame = new UiFrame(50) { Anchor = Anchor.Center, Height = 400, Width = 400, Color = new(17, 17, 17), Alpha = .8f};
            mBackgroundLayer.AddChild(buttonFrame);

            var _continue = new UiButton(TextureRegistries.button, "Resume") { Anchor = Anchor.CenterV, RelY = .1f, RelWidth = .8f, OnClickAction = () => mLayerManager.PopLayer(), TextAllign = TextAllign.Center };
            var save = new UiButton(TextureRegistries.button, "Save") { Anchor = Anchor.CenterV, RelY = .32f, RelWidth = .8f, OnClickAction = null, TextAllign = TextAllign.Center };
            var settings = new UiButton(TextureRegistries.button, "Settings") { Anchor = Anchor.CenterV, RelY = .54f, RelWidth = .8f, OnClickAction = () => mLayerManager.AddLayer(new SettingsLayer(false)), TextAllign = TextAllign.Center };
            var menue = new UiButton(TextureRegistries.button, "Menue") { Anchor = Anchor.CenterV, RelY = .76f, RelWidth = .8f, OnClickAction = Menue, TextAllign = TextAllign.Center };

            buttonFrame.AddChild(_continue); mButtonInputTracer.AddButton(_continue); ;
            buttonFrame.AddChild(save); mButtonInputTracer.AddButton(save);
            buttonFrame.AddChild(settings); mButtonInputTracer.AddButton(settings);
            buttonFrame.AddChild(menue); mButtonInputTracer.AddButton(menue);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mBackgroundLayer.Initialize(mGraphicsDevice.Viewport.Bounds, mLayerManager.mResolutionManager.ActualResolution.UiScaling);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds, mLayerManager.mResolutionManager.ActualResolution.UiScaling);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => mLayerManager.PopLayer());
            mButtonInputTracer.Trace(inputState);
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.mResolutionManager.ActualResolution.UiScaling);
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
