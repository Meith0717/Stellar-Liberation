// MainMenueLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.UserInterface;

namespace StellarLiberation.Game.Layers.MenueLayers
{
    public class MainMenueLayer : Layer
    {
        public MainMenueLayer(Game1 game1) : base(game1, false)
        {
            var mainFrame = new UiCanvas() { RelWidth = 1, RelHeight = 1};
            mainFrame.AddChild(new UiSprite("menueBackground") { FillScale = FillScale.FillIn, Anchor = Anchor.Center });
            mainFrame.AddChild(new UiText("brolink", "Stellar\nLiberation", 1) { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });
            mainFrame.AddChild(new UiButton("button", "New Game") { VSpace = 20, HSpace = 20, RelY = .5f, OnClickAction = () => LayerManager.AddLayer(new GameState(Game1)) });

            mainFrame.AddChild(new UiButton("button", "Continue")
            {
                VSpace = 20,
                HSpace = 20,
                RelY = .6f,
                OnClickAction = () =>
            {
                LayerManager.AddLayer(new LoadingLayer(Game1, null));
                PersistanceManager.LoadAsync(new GameState(Game1), PersistanceManager.GameSaveFilePath, (gameState) => { LayerManager.PopLayer(); LayerManager.AddLayer(gameState); }, (ex) => throw ex);
            }
            });

            mainFrame.AddChild(new UiButton("button", "Settings") { VSpace = 20, HSpace = 20, RelY = .7f, OnClickAction = () => LayerManager.AddLayer(new SettingsLayer(Game1)) });

            mainFrame.AddChild(new UiButton("copyright", "") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = null });

            mainFrame.AddChild(new UiButton("button", "Exit Game") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => LayerManager.Exit() });
            AddUiElement(mainFrame);
        }

        public override void Destroy()
        {
            base.Destroy();
            SoundEffectManager.Instance.StopAllSounds();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            inputState.DoAction(ActionType.ESC, LayerManager.Exit);
        }
    }
}
