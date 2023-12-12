﻿// PauseLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers
{
    public class PauseLayer : Layer
    {
        private UiLayer mBackgroundLayer;

        public PauseLayer()
            : base(false)
        {
            mBackgroundLayer = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent };

            var buttonFrame = new UiFrame(50) { Anchor = Anchor.Center, Height = 700, Width = 500, Color = new(17, 17, 17) };
            mBackgroundLayer.AddChild(buttonFrame);

            var save = new UiButton(TextureRegistries.button, "Save Game") { Anchor = Anchor.CenterV, RelY = .1f, RelWidth = .8f, OnClickAction = () => mLayerManager.GameLayerFactory.SaveGameLayer(), TextAllign = TextAllign.Center };
            var settings = new UiButton(TextureRegistries.button, "Settings") { Anchor = Anchor.CenterV, RelY = .3f, RelWidth = .8f, OnClickAction = () => mLayerManager.AddLayer(new SettingsLayer(false)), TextAllign = TextAllign.Center };
            var menue = new UiButton(TextureRegistries.button, "Exit to Menue") { Anchor = Anchor.CenterV, RelY = .5f, RelWidth = .8f, OnClickAction = Menue, TextAllign = TextAllign.Center };
            var desktop = new UiButton(TextureRegistries.button, "Exit to Desktop") { Anchor = Anchor.CenterV, RelY = .7f, RelWidth = .8f, OnClickAction = () => mLayerManager.Exit(), TextAllign = TextAllign.Center };
            var resume = new UiButton(TextureRegistries.button, "Resume") { Anchor = Anchor.CenterV, RelY = .9f, RelWidth = .8f, OnClickAction = () => mLayerManager.PopLayer(), TextAllign = TextAllign.Center };

            buttonFrame.AddChild(resume);
            buttonFrame.AddChild(save);
            buttonFrame.AddChild(settings);
            buttonFrame.AddChild(menue);
            buttonFrame.AddChild(desktop);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => mLayerManager.PopLayer());
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, mLayerManager.ResolutionManager.UiScaling);
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
