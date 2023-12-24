// PauseLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public PauseLayer(GameLayer gameLayer)
            : base(false)
        {
            mBackgroundLayer = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent };

            var buttonFrame = new UiFrame(50) { Anchor = Anchor.Center, Height = 500, Width = 500, Color = new(17, 17, 17) };
            mBackgroundLayer.AddChild(buttonFrame);

            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Save Game") { Anchor = Anchor.CenterV, RelY = .05f, RelWidth = .8f, OnClickAction = () => {
                LayerManager.PopLayer();
                LayerManager.AddLayer(new LoadingLayer(false));
                mPersistanceManager.SaveGameLayerAsync(gameLayer, () =>  LayerManager.PopLayer(), (ex) => throw ex);
            }, TextAllign = TextAllign.Center });
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Settings") { Anchor = Anchor.CenterV, RelY = .23f, RelWidth = .8f, OnClickAction = () => LayerManager.AddLayer(new SettingsLayer(false)), TextAllign = TextAllign.Center });
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Exit to Menue") { Anchor = Anchor.CenterV, RelY = .41f, RelWidth = .75f, OnClickAction = Menue, TextAllign = TextAllign.Center });
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Exit to Desktop") { Anchor = Anchor.CenterV, RelY = .59f, RelWidth = .8f, OnClickAction = () => LayerManager.Exit(), TextAllign = TextAllign.Center });
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Resume") { Anchor = Anchor.CenterV, RelY = .85f, RelWidth = .8f, OnClickAction = () => LayerManager.PopLayer(), TextAllign = TextAllign.Center });
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
            inputState.DoAction(ActionType.ESC, () => LayerManager.PopLayer());
            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, LayerManager.ResolutionManager.UiScaling);
        }

        private void Menue()
        {
            LayerManager.PopLayer(); // Pause Menue
            LayerManager.PopLayer(); // Hud
            LayerManager.PopLayer(); // Game
        }
    }
}
