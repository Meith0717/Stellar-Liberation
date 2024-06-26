// PauseLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;

namespace StellarLiberation.Game.Layers.MenueLayers
{
    public class PauseLayer : Layer
    {
        public PauseLayer(GameState gameState, Game1 game1)
            : base(game1, false)
        {
            var mainFrame = new UiFrame(false) { RelHeight = 1, RelWidth = 1, Alpha = .5f };

            var buttonFrame = new UiGrid(1, 6)
            {
                Anchor = Anchor.W,
                Height = 500,
                Width = 400,
                HSpace = 20
            };
            mainFrame.AddChild(buttonFrame);

            buttonFrame.Set(0, 0, new UiButton("button", "Resume")
            {
                OnClickAction = () =>
                {
                    LayerManager.PopLayer(); SoundEffectManager.Instance.ResumeAllSounds();
                },
                Anchor = Anchor.Center,
                FillScale = FillScale.Fit
            });
            buttonFrame.Set(0, 1, new UiButton("button", "Save Game")
            {
                OnClickAction = () =>
                {
                    LayerManager.AddLayer(new LoadingLayer(Game1, false));
                    PersistanceManager.SaveAsync(PersistanceManager.GameSaveFilePath, gameState, () => LayerManager.PopLayer(), (ex) => throw ex);
                },
                Anchor = Anchor.Center,
                FillScale = FillScale.Fit
            });
            buttonFrame.Set(0, 2, new UiButton("button", "Settings")
            {
                OnClickAction = () => LayerManager.AddLayer(new SettingsLayer(Game1)),
                Anchor = Anchor.Center,
                FillScale = FillScale.Fit
            });
            buttonFrame.Set(0, 3, new UiButton("button", "Exit to Desktop")
            {
                OnClickAction = LayerManager.Exit,
                Anchor = Anchor.Center,
                FillScale = FillScale.Fit
            });
            buttonFrame.Set(0, 5, new UiButton("button", "Exit to Menue")
            {
                OnClickAction = Menue,
                Anchor = Anchor.Center,
                FillScale = FillScale.Fit
            });
            SoundEffectManager.Instance.PauseAllSounds();
            AddUiElement(mainFrame);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.PopLayer());
            base.Update(gameTime, inputState);
        }

        private void Menue()
        {
            LayerManager.PopLayer();
            LayerManager.PopLayer();
        }
    }
}
