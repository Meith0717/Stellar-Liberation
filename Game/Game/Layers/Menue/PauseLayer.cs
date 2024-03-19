// PauseLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
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
    public class PauseLayer : Layer
    {
        private readonly UiFrame mBackgroundLayer;

        public PauseLayer(GameLayerManager gameState, Game1 game1)
            : base(game1, false)
        {
            mBackgroundLayer = new() { RelHeight = 1, RelWidth = 1, Color = Color.Transparent };

            var buttonFrame = new UiFrame() { Anchor = Anchor.Center, Height = 500, Width = 400 };
            mBackgroundLayer.AddChild(buttonFrame);

            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Save Game")
            {
                Anchor = Anchor.CenterV,
                RelY = .05f,
                RelWidth = .8f,
                OnClickAction = () =>
                {
                    LayerManager.AddLayer(new LoadingLayer(Game1, false));
                    PersistanceManager.SaveAsync(PersistanceManager.GameSaveFilePath, gameState, () => LayerManager.PopLayer(), (ex) => throw ex);
                }
            });
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Settings") { Anchor = Anchor.CenterV, RelY = .2f, RelWidth = .8f, OnClickAction = () => LayerManager.AddLayer(new SettingsLayer(Game1))});
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Resume") { Anchor = Anchor.CenterV, RelY = .35f, RelWidth = .8f, OnClickAction = () => { LayerManager.PopLayer(); SoundEffectManager.Instance.ResumeAllSounds(); } });
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Exit to Menue") { Anchor = Anchor.CenterV, RelY = .7f, RelWidth = .8f, OnClickAction = Menue,});
            buttonFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Exit to Desktop") { Anchor = Anchor.CenterV, RelY = .85f, RelWidth = .8f, OnClickAction = () => LayerManager.Exit() });
            SoundEffectManager.Instance.PauseAllSounds();
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
        }

        public override void ApplyResolution() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.PopLayer());
            mBackgroundLayer.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
        }

        private void Menue()
        {
            LayerManager.PopLayer(); // Pause Menue
            LayerManager.PopLayer(); // Game
        }
    }
}
