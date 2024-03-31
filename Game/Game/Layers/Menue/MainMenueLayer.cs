// MainMenueLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
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

namespace StellarLiberation.Game.Layers.MenueLayers
{
    public class MainMenueLayer : Layer
    {
        public MainMenueLayer(Game1 game1) : base(game1, false)
        {
            var mainFrame = new UiFrame() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mainFrame.AddChild(new UiSprite(MenueSpriteRegistries.menueBackground) { FillScale = FillScale.FillIn, Anchor = Anchor.Center });

            mainFrame.AddChild(new UiText(FontRegistries.titleFont, "Stellar\nLiberation") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });

            mainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "New Game") { VSpace = 20, HSpace = 20, RelY = .5f, OnClickAction = () => LayerManager.AddLayer(new GameLayerManager(Game1)) });

            mainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Continue")
            {
                VSpace = 20,
                HSpace = 20,
                RelY = .6f,
                OnClickAction = () =>
            {
                LayerManager.AddLayer(new LoadingLayer(Game1, false));
                PersistanceManager.LoadAsync(new GameLayerManager(Game1), PersistanceManager.GameSaveFilePath, (gameState) => { LayerManager.PopLayer(); LayerManager.AddLayer(gameState); }, (ex) => throw ex);
            }
            });

            mainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Settings") { VSpace = 20, HSpace = 20, RelY = .7f, OnClickAction = () => LayerManager.AddLayer(new SettingsLayer(Game1)) });

            mainFrame.AddChild(new UiButton(MenueSpriteRegistries.copyright, "") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = null });

            mainFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Exit Game") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => LayerManager.Exit() });
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
